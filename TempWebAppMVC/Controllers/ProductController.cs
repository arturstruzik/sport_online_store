using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempWebAppMVC.Models;

namespace TempWebAppMVC.Controllers
{
    public class ProductController : Controller
    {
        StoreOnlineEntities db = new StoreOnlineEntities();
        public int PageSize = 4;

        public ViewResult List(string category, int page = 1)
        {
            IEnumerable<Product> products;
            if (string.IsNullOrEmpty(category)) 
            {
                products = db.TakeProductsWithCategory(null);
            }
            else
            {
                products = db.TakeProductsWithCategory(int.Parse(category));
            }

            ProductListViewModel model = new ProductListViewModel()
            {
                Products = products
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? db.Products.Count() : db.Products.Count(x => x.CategoryId.ToString() == category)
                },
                CurrentCategory = category
            };

            return View(model);
        }
    }
}