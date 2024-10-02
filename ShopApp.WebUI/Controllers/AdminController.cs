using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.BLL.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    [Authorize(Roles = "admin")] // Oturum açıksa çalışsın
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(IProductService productService, ICategoryService categoryService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("admin/products")]
        public IActionResult ProductList()
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            });
        }

        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel product, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                };

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        Image image = new Image();
                        image.ImageUrl = file.FileName;

                        entity.Images.Add(image);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }

                _productService.Create(entity);
                return RedirectToAction("ProductList");
            }
            return View(product);
        }

        public IActionResult EditProduct(int id)
        {
            if (id == null)
                return NotFound();


            var entity = _productService.GetProductDetails(id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Images = entity.Images,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel product, List<IFormFile> files, int[] categoryIds)
        {
            Product entity = new Product();
            entity.Id = product.Id;
            entity.Price = product.Price;
            entity.Name = product.Name;
            entity.Description = product.Description;

            if (files != null)
            {
                entity.Images = new List<Image>();
                foreach (var file in files)
                {
                    Image image = new Image();
                    image.ImageUrl = file.FileName;

                    entity.Images.Add(image);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            try
            {
                _productService.Update(entity, categoryIds);
                return RedirectToAction("ProductList");
            }
            catch (Exception)
            {

                return View(entity);
            }


        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            _productService.Delete(_productService.GetById(id));

            return RedirectToAction("ProductList");
        }

        [Route("admin/categories")]
        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = _categoryService.GetAll()
            });
        }

        public IActionResult CreateCategory()
        {
            return View(new CategoryModel());
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel cat)
        {
            Category category = new Category();
            category.Name = cat.Name;

            try
            {
                _categoryService.Create(category);
                return RedirectToAction("CategoryList");
            }
            catch (Exception)
            {
                return View(cat);
            }
        }

        [Route("admin/categories/{id?}")]
        public IActionResult EditCategory(int? id)
        {
            var category = _categoryService.GetByIdWithProducts(id.Value);


            return View(new CategoryModel()
            {
                Name = category.Name,
                Id = category.Id,
                Products = category.ProductCategories.Select(i => i.Product).ToList()
            });
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel cat)
        {
            var entity = _categoryService.GetById(cat.Id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = cat.Name;
            _categoryService.Update(entity);

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.Delete(_categoryService.GetById(id));

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);

            return Redirect("/admin/categories/" + categoryId);
        }

        public async Task<IActionResult> UserList()
        {
            List<UserModel> model = new List<UserModel>();

            foreach (var item in _userManager.Users.ToList())
            {
                UserModel user = new UserModel()
                {
                    FullName = item.FullName,
                    Username = item.UserName,
                    Email = item.Email,
                    EmailConfirmed = item.EmailConfirmed,
                    IsAdmin = await _userManager.IsInRoleAsync(item, "admin")
                };

                model.Add(user);
            }


            return View(model);
        }

        public async Task<IActionResult> UserEdit(string email)
        {
            ApplicationUser entity = await _userManager.FindByEmailAsync(email);
            if (entity == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı ile daha önce kayıt yapılmamış.");
                return View(entity);
            }

            UserModel user = new UserModel();
            user.FullName = entity.FullName;
            user.Username = entity.UserName;
            user.EmailConfirmed = entity.EmailConfirmed;
            user.Email = entity.Email;
            user.IsAdmin = await _userManager.IsInRoleAsync(entity, "admin");

            return View(user);

        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserModel model)
        {
            ApplicationUser entity = await _userManager.FindByEmailAsync(model.Email);

            if(entity == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı ile daha önce kayıt yapılmamış.");
                return View(entity);
            }

            entity.FullName = model.FullName;
            entity.UserName = model.Username;
            entity.Email = model.Email;
            entity.EmailConfirmed = model.EmailConfirmed;
            if (model.IsAdmin)
            {
                await _userManager.AddToRoleAsync(entity, "admin");
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(entity, "admin");
            }

            await _userManager.UpdateAsync(entity);
            return RedirectToAction("UserList");

        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(string email)
        {
            ApplicationUser entity = await _userManager.FindByEmailAsync(email);
            if (entity == null)
            {
                ModelState.AddModelError("", "Silme işlemi başarısız.");
                return View(entity);
            }

            await _userManager.DeleteAsync(entity);

            return RedirectToAction("userList");
        }
    }
}
