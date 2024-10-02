using Microsoft.AspNetCore.Mvc;
using ShopApp.BLL.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke() //Çağırmak
        {
            return View(new CategoryListViewModel()
            {
                SelectedCategory= RouteData.Values["category"]?.ToString(),    //[Route("products/{category?}")]
                Categories =_categoryService.GetAll()
            });
        }
    }
}
