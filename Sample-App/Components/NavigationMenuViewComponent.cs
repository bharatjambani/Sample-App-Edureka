using Microsoft.AspNetCore.Mvc;
using Sample_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Components
{
    //use IStoreRepo to get unique categories
    public class NavigationMenuViewComponent:ViewComponent
    {
        private readonly IStoreRepo _repository;

        //public string Invoke()
        //{
        //    return "Cricket ... Chess ... Soccer";
        //}

        public NavigationMenuViewComponent(IStoreRepo repository)
        {
            _repository = repository;
        }
        public IViewComponentResult Invoke()
        {
            var uniqueCategories = _repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(uniqueCategories);
        }
    }
}
