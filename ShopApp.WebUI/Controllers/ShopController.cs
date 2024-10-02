using Microsoft.AspNetCore.Mvc;
using ShopApp.BLL.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("products/{category?}")]
        public IActionResult List(string category, int page=1)
        {
            const int pageSize = 3;
            ProductListModel products = new ProductListModel() 
            { 
                PageModel = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    ItemsPerPage=pageSize,
                    CurrentCategory=category,
                    CurrentPage=page
                },
                Products = _productService.GetProductsByCategory(category,page,pageSize) };

            return View(products);
        }

        public IActionResult Details(int? id) //? : nullable
        {
            if (id == null)
            {
                return NotFound();
            }
            //Product product = _productService.GetById(id.Value);
            Product product = _productService.GetProductDetails((int)id);

            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailsModel()
            {
                Product=product,
                Categories=product.ProductCategories.Select(i=> i.Category).ToList(),
                Comments = product.Comments
            });
        }
    }
}
