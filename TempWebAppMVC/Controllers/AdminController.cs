using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempWebAppMVC.Models;

namespace TempWebAppMVC.Controllers
{
    [Authorize(Roles = "Worker")]
    public class AdminController : Controller
    {
        StoreOnlineEntities db = new StoreOnlineEntities();

        public ActionResult Index()
        {
            return View(db.Products);
        }

        [HttpGet]
        public ViewResult Edit(int productId)
        {
            Product product = db.Products.FirstOrDefault(p => p.ProductId == productId);
            ViewBag.Categories = GetCategories(db.Categories);

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                SaveProduct(product);
                if (product.Count == 0)
                {
                    TempData["message"] = string.Format("{0} został usunięty", product.Name);
                }
                else
                {
                    TempData["message"] = string.Format("{0} został zapisany", product.Name);
                }

                return RedirectToAction("Index");
            }

            ViewBag.Categories = GetCategories(db.Categories);
            return View(product);
        }

        public ViewResult Create()
        {
            ViewBag.Categories = GetCategories(db.Categories);

            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product product = db.Products.Find(productId);
            if (product != null)
            {
                DeleteProduct(product);
                TempData["message"] = string.Format("{0} został usunięty", product.Name);
            }

            return RedirectToAction("Index");
        }

        private void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                db.Products.Add(product);
            }
            else
            {
                Product dbEntry = db.Products.Find(product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.CategoryId = product.CategoryId;
                    dbEntry.Count = product.Count;
                    dbEntry.Price = product.Price;
                }
            }

            db.SaveChanges();
        }

        private void DeleteProduct(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
        }

        private Dictionary<string, string> GetCategories(IEnumerable<Category> categories)
        {
            Dictionary<string, string> dictCategories = new Dictionary<string, string>();
            dictCategories.Add("", "");

            foreach (var category in categories)
            {
                dictCategories.Add(category.CategoryId.ToString(), category.Name);
            }

            return dictCategories;
        }
    }
}