using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempWebAppMVC.Models;

namespace TempWebAppMVC.Controllers
{
    public class NavController : Controller
    {
        StoreOnlineEntities db = new StoreOnlineEntities();

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<Category> categories = db.Categories;
            
            return PartialView(categories);
        }
    }
}