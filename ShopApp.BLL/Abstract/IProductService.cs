using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.BLL.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        List<Product> GetAll(Expression<Func<Product, bool>> filter=null);
        List<Product> GetProductsByCategory(string category, int page, int pageSize);
        Product GetProductDetails(int id);
        void Create(Product entity);
        void Delete(Product entity);
        int GetCountByCategory(string category);
        void Update(Product entity, int[] categoryIds);
    }
}
