using ShopApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        
        public List<Image> Images { get; set; }

        [Required]
        public decimal Price { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        public List<Category> SelectedCategories { get; set; }

        public ProductModel()
        {
            Images = new List<Image>();
        }
    }
}
