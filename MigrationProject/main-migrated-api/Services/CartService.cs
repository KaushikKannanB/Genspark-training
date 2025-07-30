using MainMigration.Context;
using MainMigration.Interfaces;
using MainMigration.Models;

namespace MainMigration.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<int, Cart> cartrepo;
        private readonly IRepository<int, Product> prodrepo;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserService userService;
        private readonly IOrderService orderService;
        private readonly MainMigrationContext context;
        public CartService(MainMigrationContext co, IOrderService o, IRepository<int, Cart> c, IRepository<int, Product> p, ICurrentUserService cu, IUserService us)
        {
            cartrepo = c;
            prodrepo = p;
            currentUserService = cu;
            userService = us;
            orderService = o;
            context = co;
        }
        public async Task<Cart> AddtoCart(int productid)
        {
            var prod = await prodrepo.GetById(productid);
            var user = await userService.GetByUserName(currentUserService.Name);
            if (prod.isSold == "YES")
            {
                throw new Exception("Product is SOLD!");
            }
            else if (prod.UserId == user.UserId)
            {
                throw new Exception("You cannot add your own product to your cart!!");
            }
            else
            {
                Cart c = new();
                c.ProductId = productid;
                c.UserId = user.UserId;

                var cartadded = await cartrepo.Add(c);

                return cartadded;
            }
        }

        public async Task<IEnumerable<Cart>> GetMyCart()
        {
            var user = await userService.GetByUserName(currentUserService.Name);

            var allcart = await cartrepo.GetAll();

            var mycart = allcart.Where(c => c.UserId == user.UserId);

            return mycart;
        }

        public async Task<Order> BuyAllCart()
        {
            var mycart = await GetMyCart();
            List<int> prodids = new List<int>();
            bool all_available = true;

            foreach (var i in mycart)
            {
                var prod = await prodrepo.GetById(i.ProductId);
                prodids.Add(i.ProductId);
                if (prod.isSold == "YES")
                {
                    all_available = false;
                    break;
                }
            }

            if (!all_available)
            {
                throw new Exception("Some of the items in your cart has been sold!");
            }
            else
            {
                var ordered = await orderService.PlaceOrder(prodids);
                return ordered;
            }
        }
        public async Task<Cart> ExistinCart(int productid)
        {
            var mycart = await GetMyCart();
            var exist = mycart.FirstOrDefault(c => c.ProductId == productid);

            return exist;
        }
        public async Task<Cart> RemoveItemFromCart(int productid)
        {
            var mycart = await GetMyCart();

            var this_product = mycart.FirstOrDefault(c => c.ProductId == productid);

            if (this_product == null)
            {
                throw new Exception("Product already does not exist!!!");
            }

            context.Carts.Remove(this_product);

            await context.SaveChangesAsync();

            return this_product;
        }
        public async Task<IEnumerable<Cart>> EmptyCart()
        {
            var mycart = await GetMyCart();
            if (mycart != null)
            {
                context.Carts.RemoveRange(mycart);
                await context.SaveChangesAsync();
            }

            var now_mycart = await GetMyCart();
            return now_mycart;
        }

        public async Task<Order> BuySpecificItemFromCart(int productid)
        {
            var mycart = await GetMyCart();

            var this_product = mycart.FirstOrDefault(c => c.ProductId == productid);

            if (this_product == null)
            {
                throw new Exception("Product doesnt exist in your cart");
            }
            else
            {
                List<int> prodids = new List<int>();
                prodids.Add(this_product.ProductId);
                var ordered = await orderService.PlaceOrder(prodids);

                return ordered;
            }
        }
    }
}