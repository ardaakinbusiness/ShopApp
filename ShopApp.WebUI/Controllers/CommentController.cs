using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.BLL.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class CommentController : Controller
    {
        private IProductService _productService;
        private ICommentService _commentService;
        private UserManager<ApplicationUser> _userManager;

        public CommentController(IProductService productService, ICommentService commentService,UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public IActionResult ShowProductComments(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = _productService.GetProductDetails(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView("_PartialComments",product.Comments);
        }

        [HttpPost]
        public IActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Comment comment = _commentService.GetById(id.Value);

            if (comment == null)
            {
                return NotFound();
            }

            comment.Text = text;
            comment.CreateOn = DateTime.Now;

            try
            {
                _commentService.Update(comment);

                return Json(new { result = true });
            }
            catch (Exception)
            {

                return Json(new { result = false });
            }         
            
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Comment comment = _commentService.GetById(id.Value);

            if (comment == null)
            {
                return NotFound();
            }

            try
            {
                _commentService.Delete(comment);

                return Json(new { result = true });
            }
            catch (Exception)
            {

                return Json(new { result = false });
            }

        }

        [HttpPost]
        public IActionResult Create(int? productid, CommentModel model)
        {
            if (productid == null)
            {
                return BadRequest();
            }

            Product product = _productService.GetById(productid.Value);

            if (product == null)
            {
                return NotFound();
            }
                        
            Comment comment = new Comment();
            comment.ProductId = product.Id;
            comment.Text = model.Text;
            comment.UserId = _userManager.GetUserId(User);
            comment.CreateOn = DateTime.Now;
                    

            try
            {
                _commentService.Create(comment);

                return Json(new { result = true });
            }
            catch (Exception)
            {

                return Json(new { result = false });
            }

        }
    }
}
