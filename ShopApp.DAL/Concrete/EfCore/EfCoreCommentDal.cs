using ShopApp.DAL.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DAL.Concrete.EfCore
{
    public class EfCoreCommentDal:EfCoreGenericRepository<Comment,ShopContext>,ICommentDal
    {
    }
}
