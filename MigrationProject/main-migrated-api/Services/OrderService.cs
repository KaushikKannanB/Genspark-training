using MainMigration.Context;
using MainMigration.Interfaces;
using MainMigration.Models;

namespace MainMigration.Services
{
    public class OrderService : IOrderService
    {
        private readonly MainMigrationContext context;
        private readonly IRepository<int, Product> prodrepo;
        private readonly IRepository<int, Order> orderrepo;
        private readonly IRepository<int, OrderDetail> orderdetailrepo;
        private readonly IRepository<int, Cart> cartrepo;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserService userService;
        public OrderService(IRepository<int, Cart> ca, IUserService u, ICurrentUserService cur, MainMigrationContext co, IRepository<int, Order> o, IRepository<int, OrderDetail> or, IRepository<int, Product> pr)
        {
            orderrepo = o;
            orderdetailrepo = or;
            prodrepo = pr;
            context = co;
            currentUserService = cur;
            userService = u;
            cartrepo = ca;
        }

        public async Task<Order> PlaceOrder(IEnumerable<int> productIds)
        {
            if (productIds.Count() == 0)
            {
                throw new Exception("Nothing to order, empty order not valid");
            }
            bool all_available = true;
            var user = await userService.GetByUserName(currentUserService.Name);
            foreach (int i in productIds)
            {
                var prod = await prodrepo.GetById(i);

                if (prod == null || prod.isSold == "YES")
                {
                    all_available = false;
                    break;
                }
            }

            if (!all_available)
            {
                throw new Exception("All products are not available");
            }
            else
            {


                //checking userid not equal for buy and sell...
                foreach (int i in productIds)
                {
                    var prod = await prodrepo.GetById(i);
                    if (user.UserId == prod.UserId)
                    {
                        throw new Exception("You cannott buy your own product");
                    }
                }
                
                Order o = new();
                o.OrderDate = DateTime.UtcNow;
                foreach (int i in productIds)
                {
                    var prod = await prodrepo.GetById(i);
                    prod.isSold = "YES";
                    await context.SaveChangesAsync();
                    o.TotalAmount += prod.Price;
                }
                o.Status = "ORDERED";
                
                o.UserId = user.UserId;

                var ordered = await orderrepo.Add(o);

                foreach (int i in productIds)
                {
                    var prod = await prodrepo.GetById(i);

                    OrderDetail od = new();
                    od.OrderID = ordered.OrderID;
                    od.ProductID = prod.ProductId;
                    od.Price = prod.Price;

                    var od_added = await orderdetailrepo.Add(od);

                    var allcart = await cartrepo.GetAll();
                    var this_product = allcart.Where(c => c.ProductId == i);

                    if (this_product != null)
                    {
                        context.Carts.RemoveRange(this_product);
                        await context.SaveChangesAsync();
                    }
                }
                //if in cart - delete


                return ordered;

            }

        }

        public async Task<IEnumerable<Order>> GetMyOrders()
        {
            var user = await userService.GetByUserName(currentUserService.Name);

            var all_orders = await orderrepo.GetAll();

            var my_orders = all_orders.Where(o => o.UserId == user.UserId);

            return my_orders;
        }

        public async Task<IEnumerable<OrderDetail>> GetMyOrderDetails(int orderid)
        {
            var myorders = await GetMyOrders();

            var this_order = myorders.FirstOrDefault(o => o.OrderID == orderid);
            if (this_order == null)
            {
                throw new Exception("This order doesnt belong to the user!!!!");
            }

            var alldetails = await orderdetailrepo.GetAll();
            var mydetails = alldetails.Where(o => o.OrderID == orderid);

            return mydetails;
        }
    }
}