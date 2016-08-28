using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TempWebAppMVC.Models;

namespace TempWebAppMVC.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.Login, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Login, false);
                    return RedirectToAction("List", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Dane logowania są niepoprawne");
                }
            }
            
            return View(user);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new StoreOnlineEntities())
                {
                    var crypto = new SimpleCrypto.PBKDF2();

                    var enctpPass = crypto.Compute(user.Password);

                    if (!db.Users.Any(x => x.Login == user.Login))
                    {
                        db.InsertUser(user.Login, enctpPass, crypto.Salt, 1);
                        db.SaveChanges();

                        return RedirectToAction("List", "Product");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Podany login już istnieje. Wybierz inny");
                    }
                }
            }

            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("List", "Product");
        }

        [Authorize(Roles = "Worker,User")]
        public ViewResult Orders()
        {
            var db = new StoreOnlineEntities();

            return View(db.Orders);
        }

        private bool IsValid(string login, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            
            bool isValid = false;

            using (var db = new StoreOnlineEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == login);
                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }
    }
}