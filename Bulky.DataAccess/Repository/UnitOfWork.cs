using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BulkyBookWebDbContext dbContext;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(BulkyBookWebDbContext dbContext) 
        {
            this.dbContext = dbContext;
            Category=new CategoryRepository(dbContext);
            Product = new ProductRepository(dbContext);
            Company = new CompanyRepository(dbContext);
            ShoppingCart = new ShoppingCartRepository(dbContext);
            ApplicationUser = new ApplicationUserRepository(dbContext);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
