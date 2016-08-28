using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempWebAppMVC.Models
{
    public class CartModel
    {
        private List<CartLineModel> lines = new List<CartLineModel>();

        public void AddItem(Product product, int quantity)
        {
            CartLineModel line = lines.FirstOrDefault(x => x.Product.ProductId == product.ProductId);

            if (line == null)
            {
                lines.Add(new CartLineModel()
                {
                    Product = product, Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveItem(Product product)
        {
            lines.RemoveAll(x => x.Product.ProductId == product.ProductId);
        }

        public void Clear()
        {
            lines.Clear();
        }

        public decimal ComputeTotalValue()
        {
            return lines.Sum(x => x.Product.Price*x.Quantity);
        }

        public IEnumerable<CartLineModel> Lines
        {
            get { return lines; }
        }
    }

    public class CartLineModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}