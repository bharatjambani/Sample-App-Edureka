using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Sample_App.Models;
using Sample_App.Services;
using Sample_App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Controllers
{
    [Authorize(Roles = "Administrator,User")]
    //[Route("Account")]
    public class HomeController : Controller
    {
        //private readonly IRandomService _randomService;
        //private readonly IRandomWrapper _randomWrapper;
        private readonly IStoreRepo _repo;
        private readonly ILogger<HomeController> _logger;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        public int PageSize = 4;

        public HomeController(IStoreRepo repo, ILogger<HomeController> logger, IFileProvider fileProvider, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }
        //public HomeController(IRandomService randomService, IRandomWrapper randomWrapper)
        //{
        //    _randomService = randomService;
        //    _randomWrapper = randomWrapper;
        //}
        public IActionResult Index(string category, int productPage = 1)
        {
            //string result = $"Random Wrapper : {_randomWrapper.GetNumber()} RandomService Data: {_randomService.GetNumber()}";
            //ViewBag.Result = result;
            //return View();
            //return Content(result);

            _logger.LogInformation($"Index Action called {category} for {productPage}");
            var products = _repo.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize);

            var pagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null ? _repo.Products.Count() : _repo.Products.Where(c => c.Category == category).Count()
                //TotalItems = _repo.Products.Count() 
            };

            var productListViewModel = new ProductListViewModel
            { Products = products, PagingInfo = pagingInfo, CurrentCategory = category };
            _logger.LogInformation($"ProductListViewModel created");

            //return View(products);
            return View(productListViewModel);
        }

        public IActionResult Details(int id)
        {
            var product = _repo.Details(id);
            return View(product);
        }

        public IActionResult Update(int id)
        {
            var product = _repo.Details(id);

            //BJ:Replace with mapper
            //ProductEditModel prodEditModel = new ProductEditModel
            //{
            //    Category = product.Category,
            //    Description = product.Description,
            //    MfgDate = product.MfgDate,
            //    Name = product.Name,
            //    Price = product.Price,
            //    ProductID = product.ProductID
            //};

            var prodEditModel = _mapper.Map<ProductEditModel>(product);
            return View(prodEditModel);
        }

        public IActionResult Delete(int id)
        {
            _repo.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        public IActionResult AboutUs()
        {
            //throw new Exception();
            return View();
        }
        public IActionResult ContactUs()
        {
            var contents = _fileProvider.GetDirectoryContents("KYC");
            return View(contents);
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> FileUpload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "KYC", formFile.FileName);
                    filePaths.Add(path);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePaths });
        }


        public IActionResult PerformTran()
        {
            try
            {
                _repo.PerformTran();
                return Content("Transaction Succes");
            }
            catch (Exception)
            {
                return Content("Transaction Failed");
                //throw;
            }
            //return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductEditModel prod)
        {
            if (ModelState.IsValid)
            {
                //BJ:Below code is not needed as we used mapper package
                //Product product = new Product
                //{
                //    Category = prod.Category,
                //    Description = prod.Description,
                //    MfgDate = prod.MfgDate,
                //    Name = prod.Name,
                //    Price = prod.Price,
                //    ProductID = prod.ProductID
                //};

                var product = _mapper.Map<Product>(prod);

                _repo.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Update(ProductEditModel prod)
        {
            if (ModelState.IsValid)
            {
                //BJ:Below code is replace with mapper
                //Product product = new Product
                //{
                //    Category = prod.Category,
                //    Description = prod.Description,
                //    MfgDate = prod.MfgDate,
                //    Name = prod.Name,
                //    Price = prod.Price,
                //    ProductID = prod.ProductID
                //};

                var product = _mapper.Map<Product>(prod);
                _repo.UpdateProduct(product.ProductID, product);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult VerifyCategory(string category)
        {
            if (category == "Cricket" || category == "Soccer")
            {
                return Json(true);
            }
            else
            {
                return Json($"only Cricket and Soccer categories are allowed");
            }
        }
    }
}
