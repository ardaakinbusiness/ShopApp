using Microsoft.EntityFrameworkCore;
using ShopApp.DAL.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DAL.Concrete.EfCore
{
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
    {
        public void DeleteFromCategory(int categoryId, int productId)
        {
            using (var context = new ShopContext())
            {
                var code = @" delete from ProductCategory where ProductId=@p0 and CategoryId=@p1";

                context.Database.ExecuteSqlRaw(code, productId, categoryId);
            }
        }

        public Category GetByIdWithProducts(int id)
        {
           using(var context = new ShopContext())
            {
                return context.Categories
                        .Where(i => i.Id == id)
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Product)
                        .ThenInclude(i => i.Images)
                        .FirstOrDefault();
            }
        }
    }
}
