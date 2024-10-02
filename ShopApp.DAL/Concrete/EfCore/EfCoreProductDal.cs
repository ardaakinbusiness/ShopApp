using Azure;
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
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public override IEnumerable<Product> GetAll(Expression<Func<Product, bool>> filter = null) // i=> i.CategoryId==1
        {
            using(var context = new ShopContext())
            {
                var products = context.Products.Include("Images").AsQueryable();

                if (filter != null)
                {
                    products = products.Where(filter);
                }

                return products.ToList();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                       .Include(i => i.ProductCategories)
                       .ThenInclude(i => i.Category)
                       .Where(i => i.ProductCategories.Any(c => c.Category.Name.ToLower() == category.ToLower()));
                }

                return products.ToList().Count;
            }
        }

        //public override Product GetById(int id)
        //{
        //    using (var context = new ShopContext())
        //    {
        //        return context.Products.Include("Images").FirstOrDefault(i => i.Id == id);
        //    }
        //}

        public Product GetProductDetails(int id)
        {
            using(var context = new ShopContext())
            {
                return context.Products
                        .Where(i => i.Id == id)
                        .Include("Images")
                        .Include("Comments")
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string category,int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Include(i => i.Images).AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                       .Include(i => i.ProductCategories)
                       .ThenInclude(i => i.Category)
                       .Where(i => i.ProductCategories.Any(c => c.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products
                        .Include(i=> i.ProductCategories)
                        .ThenInclude(i=> i.Category)
                        .Include(i=> i.Images)
                        .FirstOrDefault(i => i.Id == entity.Id);

                if (product != null)
                {
                    

                    product.Price = entity.Price;
                    product.Name = entity.Name;
                    product.Description = entity.Description;
                    product.ProductCategories = categoryIds.Select(i => new ProductCategory()
                    {
                        CategoryId = i,
                        ProductId = entity.Id
                    }).ToList();


                    if (entity.Images.Count > 0)
                    {
                        var silinenResim = context.Images.Where(i => i.ProductId == product.Id).ToList();
                        context.Images.RemoveRange(silinenResim);
                        product.Images = entity.Images;
                    }
                       
                }

                context.SaveChanges();                            
            }
            
        }
    }
}
