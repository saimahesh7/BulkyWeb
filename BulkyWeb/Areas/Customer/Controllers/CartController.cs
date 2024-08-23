using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.ProductCount <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.ProductCount >= 51 && shoppingCart.ProductCount <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartList = unitOfWork.ShoppingCart.GetAll(x => x.UserId == userId, includeProperties: "Product");

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = shoppingCartList,
            };

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Product.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.OrderTotal += (cart.Product.Price * cart.ProductCount);
            }

            return View(shoppingCartVM);
        }

        public IActionResult Increment(int cartId)
        {
            var cardDb = unitOfWork.ShoppingCart.Get(x => x.ShoppingCartId == cartId);
            cardDb.ProductCount += 1;

            unitOfWork.ShoppingCart.Update(cardDb);
            unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrement(int cartId)
        {
            var cardDb = unitOfWork.ShoppingCart.Get(x => x.ShoppingCartId == cartId);

            if (cardDb.ProductCount <= 1)
            {
                unitOfWork.ShoppingCart.Remove(cardDb);
                unitOfWork.Save();
            }
            else
            {
                cardDb.ProductCount -= 1;

                unitOfWork.ShoppingCart.Update(cardDb);
                unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveProduct(int cartId)
        {
            var cardDb = unitOfWork.ShoppingCart.Get(x => x.ShoppingCartId == cartId);

            unitOfWork.ShoppingCart.Remove(cardDb);
            unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            return View();
        }
    }
}
