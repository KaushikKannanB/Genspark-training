using MainMigration.Interfaces;
using MainMigration.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainMigration.Controllers
{
    [ApiController]
    [Route("api/others")]
    public class OtherController : ControllerBase
    {
        private readonly IRepository<int, Model> modelrepo;
        private readonly IRepository<int, Color> colorrepo;

        private readonly IRepository<int, Category> categrepo;
        private readonly IOtherServices otherServices;

        public OtherController(IOtherServices o,
        IRepository<int, Model> m, IRepository<int, Color> c, IRepository<int, Category> ca)
        {
            otherServices = o;
            modelrepo = m;
            colorrepo = c;
            categrepo = ca;
        }

        #region Colors
        [HttpPost("post-color")]
        public async Task<IActionResult> PostColor(string colorname)
        {
            Color c = new();
            c.ColorName = colorname.ToLower();

            var color = await otherServices.GetColorByName(c.ColorName);
            if (color == null)
            {
                var added = await colorrepo.Add(c);
                return Ok(added);
            }
            return BadRequest("Color already exists!");

        }

        [HttpGet("get-color-by-name")]
        public async Task<IActionResult> GetColorByName(string name)
        {
            var result = await otherServices.GetColorByName(name);
            if (result == null)
            {
                return BadRequest("No such color exists!");
            }
            return Ok(result);
        }

        [HttpGet("get-all-colors")]
        public async Task<IActionResult> GetAllColors()
        {
            var all = await colorrepo.GetAll();
            return Ok(all);
        }
        #endregion

        #region Category
        [HttpPost("post-category")]
        public async Task<IActionResult> PostCategory(string catname)
        {
            Category c = new();
            c.Name = catname.ToLower();

            var cat = await otherServices.GetColorByName(c.Name);
            if (cat == null)
            {
                var added = await categrepo.Add(c);
                return Ok(added);
            }
            return BadRequest("Category already exists!");

        }

        [HttpGet("get-category-by-name")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var result = await otherServices.GetCategoryByName(name);
            if (result == null)
            {
                return BadRequest("No such category exists!");
            }
            return Ok(result);
        }

        [HttpGet("get-all-category")]
        public async Task<IActionResult> GetAllCategory()
        {
            var all = await categrepo.GetAll();
            return Ok(all);
        }

        #endregion


        #region Model
        [HttpPost("post-model")]
        public async Task<IActionResult> PostModel(string name)
        {
            Model c = new();
            c.ModelName = name.ToLower();

            var cat = await otherServices.GetModelByName(c.ModelName);
            if (cat == null)
            {
                var added = await modelrepo.Add(c);
                return Ok(added);
            }
            return BadRequest("model already exists!");

        }

        [HttpGet("get-model-by-name")]
        public async Task<IActionResult> GetModelByName(string name)
        {
            var result = await otherServices.GetModelByName(name);
            if (result == null)
            {
                return BadRequest("No such model exists!");
            }
            return Ok(result);
        }

        [HttpGet("get-all-model")]
        public async Task<IActionResult> GetAllModel()
        {
            var all = await modelrepo.GetAll();
            return Ok(all);
        }

        #endregion
    }
}