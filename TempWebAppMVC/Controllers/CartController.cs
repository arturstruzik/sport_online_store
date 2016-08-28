using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TempWebAppMVC.Models;

namespace TempWebAppMVC.Controllers
{
    [Authorize(Roles = "Worker,User")]
    public class CartController : Controller
    {
        StoreOnlineEntities db = new StoreOnlineEntities();

        public ViewResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View(GetCart());
        }

        public RedirectToRouteResult AddToCart(int productId, string returnUrl)
        {
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                CartModel cart = GetCart();
                cart.AddItem(product, 1);

                if (cart.Lines.FirstOrDefault(x => x.Product.ProductId == productId) != null)
                {
                    TempData["message"] = string.Format("{0} został dodany do koszyka", product.Name);
                }
            }

            return RedirectToAction("Index", new { returnUrl} );
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                CartModel cart = GetCart();
                cart.RemoveItem(product);

                if (cart.Lines.FirstOrDefault(x => x.Product.ProductId == productId) == null)
                {
                    TempData["message"] = string.Format("{0} został usunięty z koszyka", product.Name);
                }
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult ClearCart(string returnUrl)
        {
            GetCart().Clear();

            if (GetCart().Lines.Count() == 0)
            {
                TempData["message"] = "Wyczyszczono koszyk";
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ActionResult Checkout()
        {
            var user = db.Users.FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
            {
                TempData["message_error"] = "Błąd aplikacji";
                return RedirectToAction("Logout", "Account");
            }

            var totalPrice = GetCart().ComputeTotalValue();
            if (totalPrice <= 0)
            {
                TempData["message_error"] = "Aby przejść do kasy należy wybrać do koszyka conajmniej jeden produkt";
                return RedirectToAction("List", "Product");
            }

            return View(new ShippingDetailsViewModel()
            {
                TotalPrice = totalPrice,
                User = user
            });
        }

        public ViewResult Completed(int userId)
        {
            //zapis do tabeli orders
            var order = new Order
            {
                Date = DateTime.Today, 
                Status = "Zamowione", 
                UserId = userId
            };

            db.Orders.Add(order);
            db.SaveChanges();

            //zapis do tabeli carts
            foreach (var line in GetCart().Lines)
            {
                var cartLine = new Cart
                {
                    ProductId = line.Product.ProductId,
                    Count = line.Quantity,
                    UserId = userId,
                    OrderId = db.Orders.Max(x => x.OrderId)
                };

                db.Carts.Add(cartLine);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception) { }

            TempData["message"] = "Dokonano zakupu";
            return View();
        }

        private CartModel GetCart()
        {
            CartModel cart = (CartModel)Session["Cart"];
            if (cart == null)
            {
                cart = new CartModel();
                Session["Cart"] = cart;
            }

            return cart;
        }
    }
}