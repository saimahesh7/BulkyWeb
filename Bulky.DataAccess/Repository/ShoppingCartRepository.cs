using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly BulkyBookWebDbContext dbContext;

        public ShoppingCartRepository(BulkyBookWebDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            dbContext.ShoppingCarts.Update(shoppingCart);
        }
    }
}
