using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Models
{
    public interface IStoreRepo
    {
        IEnumerable<Product> Products { get; }

        void AddProduct(Product prod);

        bool DeleteProduct(int id);

        bool UpdateProduct(int id, Product prod);

        Product Details(int id);

        void PerformTran();

    }
}
