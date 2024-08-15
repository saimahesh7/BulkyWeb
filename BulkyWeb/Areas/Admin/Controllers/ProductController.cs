using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BulkyBookWebDbContext dbContext;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment, BulkyBookWebDbContext dbContext)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var productList = unitOfWork.Product.GetAll().ToList();
            
            return View(productList);
        }

        public IActionResult UpsertProduct(int? id)
        {
            IEnumerable<SelectListItem> categoryList = unitOfWork.Category
                .GetAll().Select(x => new SelectListItem
            {
               Text = x.Name,
               Value=x.CategoryId.ToString(),
            });

            ProductVM productVm = new ProductVM()
            {
                Product = new Product(),
                CategoryList = categoryList,
            };

            if(id == null || id == 0)
            {
                return View(productVm);
            }
            else
            {
                productVm.Product = unitOfWork.Product.Get(p => p.ProductId == id);
                 
                if(productVm.Product == null)
                {
                    return NotFound();
                }
                return View(productVm);
            }
            
        }

        [HttpPost]
        public IActionResult UpsertProduct(ProductVM productVM, IFormFile? file)
             {
             if (ModelState.IsValid)
             {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                if(file != null)
                {
                    string productPath = Path.Combine(wwwRootPath, @"images\products");
                    string fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"images\products\" + fileName;
                }

                unitOfWork.Product.Add(productVM.Product);
                unitOfWork.Save();
                TempData["success"] = "Product Successfully Created";
                return RedirectToAction("Index");
             }
             else
             {
                productVM.CategoryList = unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CategoryId.ToString(),
                });
                return View(productVM);
             }
        }

       /* public IActionResult UpdateProduct(int? id)
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
        }*/

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
