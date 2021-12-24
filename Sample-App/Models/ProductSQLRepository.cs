using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Models
{
    public class ProductSQLRepository : IStoreRepo
    {
        private readonly BankOfAmericaContext _context;

        public ProductSQLRepository(BankOfAmericaContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> Products => _context.Products;

        public void AddProduct(Product prod)
        {
            _context.Products.Add(prod);
            _context.SaveChanges();
        }

        public bool DeleteProduct(int id)
        {
            var prodToDelete = _context.Products.Single(p => p.ProductID == id);
            _context.Products.Remove(prodToDelete);
            _context.SaveChanges();
            return true;
        }

        public Product Details(int id)
        {
            var prodDetails = _context.Products.Single(p => p.ProductID == id);
            return prodDetails;
        }

        public void PerformTran()
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    //Entry in Sales table
                    //Reduce count by 1 in Qty in Inventories table

                    _context.Sales.Add(new Sale { ProductID = 2, ProductName = "Samsung" });

                    var inv = _context.Inventories.Single(p => p.ProductID == 2);
                    inv.Qty--;
                    _context.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {

                    tran.Rollback();
                    throw ex;
                }
            }
        }

        public bool UpdateProduct(int id, Product prod)
        {

            Product prdToUpdate = _context.Products.Single(p => p.ProductID == id);
            prdToUpdate.Category = prod.Category;
            prdToUpdate.Description = prod.Description;
            prdToUpdate.MfgDate = prod.MfgDate;
            prdToUpdate.Name = prod.Name;
            prdToUpdate.Price = prod.Price;
            _context.SaveChanges();
            return true;
        }
    }
}
