using MainMigration.Interfaces;
using MainMigration.Models;
using MainMigration.Models.DTOs;

namespace MainMigration.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<int, Product> prodrepo;
        private readonly IOtherServices otherServices;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserService userService;

        public ProductService(IRepository<int, Product> p, IOtherServices o, ICurrentUserService cur, IUserService us)
        {
            prodrepo = p;
            otherServices = o;
            currentUserService = cur;
            userService = us;
        }

        public async Task<IEnumerable<Object>> GetByproductname(string name)
        {
            var all = await prodrepo.GetAll();
            var res = all.Where(p => p.ProductName.ToLower() == name.ToLower() && p.isSold == "NO").
            Select(pr => new
            {
                id = pr.ProductId,
                name = pr.ProductName,
               price = pr.Price,
                color = pr.ColorId,
               years = pr.YearsUsed
            });

            return res;
        }

        public async Task<Product> AddProduct(AddProductDTO request)
        {
            var category = await otherServices.GetCategoryByName(request.Category.ToLower());
            var color = await otherServices.GetColorByName(request.Color.ToLower());
            var model = await otherServices.GetModelByName(request.Model.ToLower());

            if (category == null || model == null || color == null)
            {
                return null;
            }

            var user = await userService.GetByUserName(currentUserService.Name.ToLower());

            // Handle image upload
            string imagePath = null;
            if (request.Image != null && request.Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Create folder if not exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                imagePath = "/images/" + fileName;
            }

            Product p = new()
            {
                ProductName = request.ProductName,
                Image = imagePath,
                Price = request.Price,
                CategoryId = category.CategoryId,
                ColorId = color.ColorId,
                ModelId = model.ModelId,
                YearsUsed = request.YearsUsed,
                isSold = "NO",
                UserId = user.UserId,
                dateAdded = DateTime.Now
            };

            var res = await prodrepo.Add(p);
            return res;
        }


        public async Task<IEnumerable<Product>> GetFilteredProducts(string cat, string color, string model, string prodname)
        {
            var all = await prodrepo.GetAll();
            if (cat != null)
            {
                var category = await otherServices.GetCategoryByName(cat.ToLower());
                if (category != null)
                    all = all.Where(p => p.CategoryId == category.CategoryId);
                else
                    all = null;
            }
            if (color != null)
            {
                var col = await otherServices.GetColorByName(color.ToLower());
                if (col != null)
                    all = all.Where(p => p.ColorId == col.ColorId);
                else
                    all = null;
            }
            if (model != null)
            {
                var mod = await otherServices.GetModelByName(model.ToLower());
                if (mod != null)
                    all = all.Where(p => p.ModelId == mod.ModelId);
                else
                    all = null;
            }

            if (prodname != null)
            {
                all = all.Where(p => p.ProductName.Contains(prodname.ToLower()));
            }
            all = all.Where(p => p.isSold == "NO");
            return all;
        }
    }
}