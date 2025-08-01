using System.Text;
using Inventory.Interfaces;
using Inventory.Models;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Inventory.Services
{
    public class FAQService : IFAQService
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserService userService;
        private readonly IRepository<string, Product> prodrepo;
        private readonly IRepository<string, StockLogging> stockrepo;
        private readonly IRepository<string, Inventories> invrepo;
        private readonly IRepository<string, Category> catrepo;
        public FAQService(HttpClient _ht, IRepository<string, Category> c, IRepository<string, Inventories> inv, ICurrentUserService cur, IUserService us, IRepository<string, Product> pro, IRepository<string, StockLogging> stock)
        {
            currentUserService = cur;
            userService = us;
            prodrepo = pro;
            stockrepo = stock;
            invrepo = inv;
            catrepo = c;
            _httpClient = _ht;
        }

        public async Task<string> GetSpecificQueries(string question)
        {
            var apiUrl = "http://127.0.0.1:5000/predict";
            var payload = new { question = question };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, payload);

                if (!response.IsSuccessStatusCode)
                {
                    return "❌ Error contacting intent classification service.";
                }

                var jsonResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                if (jsonResponse == null || !jsonResponse.ContainsKey("intent"))
                {
                    return "⚠️ Failed to classify the question intent.";
                }

                string intent = jsonResponse["intent"];
                var answerBuilder = new StringBuilder();

                switch (intent)
                {
                    case "total_sales_by_user":
                        answerBuilder.AppendLine("📊Total Sales*");
                        answerBuilder.AppendLine($"Your total sales are estimated to be: ₹{(await TotalSales()).ToString("N2")}");
                        break;

                    case "top_5_user_products":
                        answerBuilder.AppendLine("Top 5 Products You Sold");
                        var topUserProducts = await TopProductsUser();
                        int index1 = 1;
                        foreach (var p in topUserProducts)
                        {
                            answerBuilder.AppendLine($"{index1++}. {p.Name} — ₹{p.Price:N2}");
                        }
                        break;

                    case "product_add_count":
                        answerBuilder.AppendLine("Products Added Count");
                        answerBuilder.AppendLine($"You've added a total of {await ProductsAdded()} products.");
                        break;

                    case "top_5_inventory_products":
                        answerBuilder.AppendLine("Top 5 Inventory Products");
                        var topInvProducts = await TopProductsGeneral();
                        int index2 = 1;
                        foreach (var p in topInvProducts)
                        {
                            answerBuilder.AppendLine($"{index2++}. {p.Name} — ₹{p.Price:N2}");
                        }
                        break;

                    case "top_categories":
                        answerBuilder.AppendLine("Top Performing Categories:");
                        var topCategories = await TopCategoryGeneral();
                        int index3 = 1;
                        foreach (var cat in topCategories)
                        {
                            answerBuilder.AppendLine($"{index3++}. {cat.CategoryName}");
                        }
                        break;

                    case "low_stock_products":
                        answerBuilder.AppendLine("⚠️Low Stock Products");
                        var lowStock = await LowStockProducts();
                        int index4 = 1;
                        foreach (var p in lowStock)
                        {
                            answerBuilder.AppendLine($"{index4++}. {p.Name}");
                        }
                        break;

                    default:
                        answerBuilder.AppendLine("🤔 Sorry, I couldn't match your question with any known intent.");
                        break;
                }

                return answerBuilder.ToString();
            }
            catch
            {
                return "❗ Internal error in processing your request.";
            }
        }


        public async Task<double> TotalSales()
        {
            var user = await userService.GetByMail(currentUserService.Email);

            var allstocklogs = await stockrepo.GetAll();
            var mystocklogs = allstocklogs.Where(s => s.UpdatedBy == user.Id && s.NewStock < s.OldStock);

            var allprods = await prodrepo.GetAll();
            double cost = 0;
            foreach (var i in mystocklogs)
            {
                var prod = allprods.FirstOrDefault(p => p.InventoryId == i.InventoryId);

                double prodcost = prod.Price;

                int prodmoved = i.OldStock-i.NewStock;

                cost += prodcost * prodmoved;
            }

            return cost;

        }

        public async Task<IEnumerable<Product>> TopProductsUser()
        {
            var user = await userService.GetByMail(currentUserService.Email);

            var allstocklogs = await stockrepo.GetAll();
            var mystocklogs = allstocklogs.Where(s => s.UpdatedBy == user.Id);

            var allprods = await prodrepo.GetAll();
            var grouped = mystocklogs.GroupBy(log => log.InventoryId).Select(g => new
            {
                Id = g.Key,
                count = g.Count()
            }).OrderByDescending(g => g.count).Take(5).ToList();

            List<Product> prods = new List<Product>();

            foreach (var i in grouped)
            {
                var prod = allprods.FirstOrDefault(p => p.InventoryId == i.Id);
                prods.Add(prod);
            }

            return prods;
        }

        public async Task<int> ProductsAdded()
        {
            var user = await userService.GetByMail(currentUserService.Email);
            var allprods = await prodrepo.GetAll();

            var prods = allprods.Where(p => p.AddedBy == user.Id);
            return prods.Count();

        }

        public async Task<IEnumerable<Product>> TopProductsGeneral()
        {
            var allprods = await prodrepo.GetAll();

            var allstocklogs = await stockrepo.GetAll();

            var grouped = allstocklogs.GroupBy(log => log.InventoryId).Select(g => new
            {
                Id = g.Key,
                count = g.Count()
            }).OrderByDescending(g => g.count).Take(5).ToList();

            List<Product> prods = new List<Product>();

            foreach (var i in grouped)
            {
                var prod = allprods.FirstOrDefault(p => p.InventoryId == i.Id);
                prods.Add(prod);
            }

            return prods;
        }

        public async Task<IEnumerable<Product>> LowStockProducts()
        {
            var allinvs = await invrepo.GetAll();

            var lowstock = allinvs.Where(i => i.Stock < i.MinThreshold);

            var allprods = await prodrepo.GetAll();

            List<Product> lowprods = new List<Product>();

            foreach (var i in lowstock)
            {
                lowprods.Add(allprods.FirstOrDefault(p => p.InventoryId == i.Id));
            }
            return lowprods;
        }

        public async Task<IEnumerable<Category>> TopCategoryGeneral()
        {
            var allprods = await prodrepo.GetAll();

            var allstocklogs = await stockrepo.GetAll();

            var allcats = await catrepo.GetAll();

            var boo = allstocklogs.Select(g => new
            {
                id = g.InventoryId,
                cat = allprods.FirstOrDefault(p => p.InventoryId == g.InventoryId).CategoryId
            }).GroupBy(g => g.cat).Select(g => new
            {
                id = g.Key,
                count = g.Count()
            }).OrderByDescending(g => g.count).Take(5).ToList();

            List<Category> cats = new List<Category>();
            foreach (var i in boo)
            {
                cats.Add(allcats.FirstOrDefault(c => c.Id == i.id));
            }

            return cats;
        }
    }
}