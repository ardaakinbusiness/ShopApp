using Microsoft.EntityFrameworkCore;
using ShopApp.DAL.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DAL.Concrete.EfCore
{
    public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
        public Cart GetCartByUserId(string userId)
        {
           using(var context = new ShopContext())
            {
                return context.Carts
                                .Include(i => i.CartItems)
                                .ThenInclude(i => i.Product)
                                .ThenInclude(i=> i.ProductCategories)
                                .ThenInclude(i=>i.Category)
                                .Include(i=> i.CartItems)
                                .ThenInclude(i => i.Product)
                                .ThenInclude(i => i.Images)
                                .FirstOrDefault(i => i.UserId == userId);
            }
        }

        public override void Update(Cart entity)
        {
            using (var context = new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }

        public void DeleteFromCart(int cartId,int productId)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItem where CartId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(cmd,cartId, productId);
                
            }
        }

        public void ClearCart(string cartId)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItem where CartId=@p0";
                context.Database.ExecuteSqlRaw(cmd, cartId);

            }
        }
    }
}
