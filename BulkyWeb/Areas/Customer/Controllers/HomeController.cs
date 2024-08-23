using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            var shoppingCart = new ShoppingCart()
            {
                  Product = unitOfWork.Product.Get(x => x.ProductId == id, includeProperties: "Category"),
                  ProductCount=1,
                  ProductId=id
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.UserId = userId;

            ShoppingCart cartfromDb = unitOfWork.ShoppingCart.Get(x => x.ProductId == shoppingCart.ProductId &&  x.UserId == userId);

            if(cartfromDb != null)
            {
                //Update Cart record
                cartfromDb.ProductCount += shoppingCart.ProductCount;
                unitOfWork.ShoppingCart.Update(cartfromDb);
            }
            else
            {
                //Add Cart Record
                unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            TempData["success"] = "Cart Updated Successfully";
            unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
