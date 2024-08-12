using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly BulkyBookWebDbContext dbContext;

        public ProductController(IUnitOfWork unitOfWork, BulkyBookWebDbContext dbContext)
        {
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var productList = unitOfWork.Product.GetAll().ToList();
            return View(productList);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Add(product);
                unitOfWork.Save();
                TempData["success"] = "Product Successfully Created";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult UpdateProduct(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("The id you are provided is irrelevent");
            }

            Product? product = unitOfWork.Product.Get(x => x.ProductId == id);
            //Product? product1= dbContext.Categories.Find(id);
            //Product? product2= dbContext.Categories.Where(x => x.ProductId == id).FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Update(product);
                unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult RemoveProduct(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("The Id you are provided is not Valid");
            }

            var product = unitOfWork.Product.Get(x => x.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult RemoveProduct(Product product)
        {
            var productDomain = unitOfWork.Product.Get(x => x.ProductId == product.ProductId);

            if (product == null)
            {
                return NotFound();
            }
            unitOfWork.Product.Remove(productDomain);
            unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}
