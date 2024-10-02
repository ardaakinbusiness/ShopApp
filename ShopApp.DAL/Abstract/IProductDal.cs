using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DAL.Abstract
{
    //public olmak zorundadır. Çünkü bir arayüzdür ve oluşturulma amacı herkesin bu arayüzü kullanmasıdır.
    //interface method: bir gövde olmaz. Her interface implement eden class kendisi o methodun ne iş yapacağını belirler. 


    public interface IProductDal:IRepository<Product>
    {
        int GetCountByCategory(string category);
        Product GetProductDetails(int id);
        void Update(Product entity, int[] categoryIds);
        List<Product> GetProductsByCategory(string category, int page, int pageSize);
    }
}
