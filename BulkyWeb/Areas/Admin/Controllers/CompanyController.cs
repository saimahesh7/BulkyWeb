using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly BulkyBookWebDbContext dbContext;

        public CompanyController(IUnitOfWork unitOfWork,BulkyBookWebDbContext dbContext)
        {
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var companyList = unitOfWork.Company.GetAll().ToList();

            return View(companyList);
        }

        public IActionResult UpsertCompany(int? id)
        {
            if (id == null)
            {
                //Create Company
                var company = new Company();

                return View(company);
            }
            else
            {
                //Update Company
                var company = unitOfWork.Company.Get(c => c.CompanyId == id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult UpsertCompany(Company company)
        {
            if (ModelState.IsValid)
            {
                if(company.CompanyId == 0 || company.CompanyId == null)
                {
                    //Create Company
                    unitOfWork.Company.Add(company);
                    unitOfWork.Save();
                    TempData["success"] = "Company Successfully Created";

                    return RedirectToAction("Index");
                }
                else
                {
                    //Update Company
                    unitOfWork.Company.Update(company);
                    unitOfWork.Save();
                    TempData["success"] = "Company Successfully Updated";

                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(company);
            }
        }

        public IActionResult RemoveCompany(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var company = unitOfWork.Company.Get(c =>c.CompanyId == id);

            return View(company);
        }

        [HttpPost]
        public IActionResult RemoveCompany(Company company)
        {
            unitOfWork.Company.Remove(company);
            unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
