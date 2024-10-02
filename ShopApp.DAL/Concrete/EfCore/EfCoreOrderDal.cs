using Microsoft.EntityFrameworkCore;
using ShopApp.DAL.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DAL.Concrete.EfCore
{
    public class EfCoreOrderDal : EfCoreGenericRepository<Order, ShopContext>, IOrderDal
    {
        public List<Order> GetOrders(string userId)
        {
            using(var context = new ShopContext())
            {
                var orders = context.Orders
                                  .Include(i => i.OrderItems)
                                  .ThenInclude(i => i.Product)
                                  .ThenInclude(i=> i.Images)
                                  .AsQueryable();

                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i => i.UserId == userId);
                }

                return orders.ToList();
            }
        }
    }
}
