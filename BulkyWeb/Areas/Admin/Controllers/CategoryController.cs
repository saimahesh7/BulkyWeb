using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly BulkyBookWebDbContext dbContext;

        public CategoryController(IUnitOfWork unitOfWork, BulkyBookWebDbContext dbContext)
        {
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categoryList = unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Category.Add(category);
                unitOfWork.Save();
                TempData["success"] = "Category Successfully Created";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult UpdateCategory(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("The id you are provided is irrelevent");
            }

            Category? category = unitOfWork.Category.Get(x => x.CategoryId == id);
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
                unitOfWork.Category.Update(category);
                unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult RemoveCategory(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("The Id you are provided is not Valid");
            }

            var category = unitOfWork.Category.Get(x => x.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult RemoveCategory(Category category)
        {
            var categoryDomain = unitOfWork.Category.Get(x => x.CategoryId == category.CategoryId);

            if (category == null)
            {
                return NotFound();
            }
            unitOfWork.Category.Remove(categoryDomain);
            unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}
