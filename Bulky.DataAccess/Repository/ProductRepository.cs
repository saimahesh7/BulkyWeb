using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository :Repository<Product> ,IProductRepository
    {
        private readonly BulkyBookWebDbContext dbContext;

        public ProductRepository(BulkyBookWebDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


        public void Update(Product product)
        {
            dbContext.Products.Update(product);

        }
    }
}
