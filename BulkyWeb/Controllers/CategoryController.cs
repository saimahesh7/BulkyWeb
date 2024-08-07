using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BulkyWebDbContext dbContext;

        public CategoryController(BulkyWebDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categoryList= dbContext.Categories.ToList();
            return View(categoryList);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
           if(ModelState.IsValid)
            {
                dbContext.Categories.Add(category);
                dbContext.SaveChanges();
                TempData["success"] = "Category Successfully Created";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult UpdateCategory(int? id)
        {
            if(id == null || id <= 0)
            {
                return BadRequest("The id you are provided is irrelevent");
            }

            Category? category= dbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            //Category? category1= dbContext.Categories.Find(id);
            //Category? category2= dbContext.Categories.Where(x => x.CategoryId == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Categories.Update(category);
                dbContext.SaveChanges();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult RemoveCategory(int? id)
        {
            if(id==null || id <= 0)
            {
                return BadRequest("The Id you are provided is not Valid");
            }

            var category = dbContext.Categories.FirstOrDefault(x => x.CategoryId==id);

            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult RemoveCategory(Category category)
        {
            var categoryDomain = dbContext.Categories.FirstOrDefault(x => x.CategoryId == category.CategoryId);

            if(category == null)
            {
                return NotFound();
            }
            dbContext.Categories.Remove(categoryDomain);
            dbContext.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}
