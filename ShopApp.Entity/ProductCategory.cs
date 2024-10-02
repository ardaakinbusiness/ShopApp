using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entity
{
    public class ProductCategory
    {
        //ManyToMany Relation (Bir kategori altında birden fazla ürün bulunabilir. Bir ürün birden fazla kategoriye ait olabilir.)
       
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
