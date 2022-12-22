using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWorkCompany.Data;
using CourseWorkCompany.Enums;
using CourseWorkCompany.Models;
using X.PagedList;

namespace CourseWorkCompany.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly Context _context;

        public CompaniesController(Context context)
        {
            _context = context;
        }

        // GET: Companies
        public IActionResult Index(CompanySort companySort, string searchKindOfActivity, string currentFilter, int? page)
        {
            if (searchKindOfActivity != null)
            {
                page = 1;
            }
            else
            {
                searchKindOfActivity = currentFilter;
            }
            ViewBag.CurrentFilter = searchKindOfActivity;
            IEnumerable<Company> context = _context.Companies;
            context = Sort(context, companySort);
            ViewBag.CurrentSort = companySort;
            context = Search(context, searchKindOfActivity);
            int pageNumber = page ?? 1;
            return View(context.ToPagedList(pageNumber, 15));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,FIO,KindOfActivity,TypeOwnership")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,FIO,KindOfActivity,TypeOwnership")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Companies == null)
            {
                return Problem("Entity set 'Context.Companies'  is null.");
            }
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<Company> Sort(IEnumerable<Company> companies, CompanySort companySort)
        {
            ViewData["Name"] = companySort == CompanySort.NameAsc ? CompanySort.NameDesc : CompanySort.NameAsc;
            ViewData["FIO"] = companySort == CompanySort.FIOAsc ? CompanySort.FIODesc : CompanySort.FIOAsc;
            companies = companySort switch
            {
                CompanySort.NameAsc => companies.OrderBy(s => s.Name),
                CompanySort.NameDesc => companies.OrderByDescending(s => s.Name),
                CompanySort.FIOAsc => companies.OrderByDescending(s => s.FIO),
                CompanySort.FIODesc => companies.OrderBy(s => s.FIO),
                _ => companies.OrderBy(s => s.CompanyId),
            };
            return companies;
        }

        private IEnumerable<Company> Search(IEnumerable<Company> companies, string searchKindOfActivity)
        {
            if (!String.IsNullOrEmpty(searchKindOfActivity))
            {
                companies = companies.Where(s => s.KindOfActivity.Contains(searchKindOfActivity));
            }
            return companies;
        }

        private bool CompanyExists(int id)
        {
          return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
