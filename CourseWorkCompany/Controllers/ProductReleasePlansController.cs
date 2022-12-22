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
    public class ProductReleasePlansController : Controller
    {
        private readonly Context _context;

        public ProductReleasePlansController(Context context)
        {
            _context = context;
        }

        // GET: ProductReleasePlans
        public IActionResult Index(ProductReleasePlanSort productReleasePlanSort, string searchCompanyName, string currentFilter, int? page)
        {
            if (searchCompanyName != null)
            {
                page = 1;
            }
            else
            {
                searchCompanyName = currentFilter;
            }
            ViewBag.CurrentFilter = searchCompanyName;
            IQueryable<ProductReleasePlan> context = _context.ProductReleasePlans.Include(p => p.Company).Include(p => p.ProductionType);
            context = Sort(context, productReleasePlanSort);
            ViewBag.CurrentSort = productReleasePlanSort;
            context = Search(context, searchCompanyName);
            int pageNumber = page ?? 1;
            return View(context.ToPagedList(pageNumber, 15));
        }

        // GET: ProductReleasePlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductReleasePlans == null)
            {
                return NotFound();
            }

            var productReleasePlan = await _context.ProductReleasePlans
                .Include(p => p.Company)
                .Include(p => p.ProductionType)
                .FirstOrDefaultAsync(m => m.ProductReleasePlanId == id);
            if (productReleasePlan == null)
            {
                return NotFound();
            }

            return View(productReleasePlan);
        }

        // GET: ProductReleasePlans/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name");
            return View();
        }

        // POST: ProductReleasePlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductReleasePlanId,CompanyId,ProductionTypeId,PlannedOutputVolume,ActualOutputVolume,QuarterInfo,YearInfo")] ProductReleasePlan productReleasePlan)
        {
            if (ModelState != null)
            {
                _context.Add(productReleasePlan);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name");
            return View(productReleasePlan);
        }

        // GET: ProductReleasePlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductReleasePlans == null)
            {
                return NotFound();
            }

            var productReleasePlan = await _context.ProductReleasePlans.FindAsync(id);
            if (productReleasePlan == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name");
            return View(productReleasePlan);
        }

        // POST: ProductReleasePlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductReleasePlanId,CompanyId,ProductionTypeId,PlannedOutputVolume,ActualOutputVolume,QuarterInfo,YearInfo")] ProductReleasePlan productReleasePlan)
        {
            if (id != productReleasePlan.ProductReleasePlanId)
            {
                return NotFound();
            }

            if (ModelState != null)
            {
                try
                {
                    _context.Update(productReleasePlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductReleasePlanExists(productReleasePlan.ProductReleasePlanId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name");
            return View(productReleasePlan);
        }

        // GET: ProductReleasePlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductReleasePlans == null)
            {
                return NotFound();
            }

            var productReleasePlan = await _context.ProductReleasePlans
                .Include(p => p.Company)
                .Include(p => p.ProductionType)
                .FirstOrDefaultAsync(m => m.ProductReleasePlanId == id);
            if (productReleasePlan == null)
            {
                return NotFound();
            }

            return View(productReleasePlan);
        }

        // POST: ProductReleasePlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductReleasePlans == null)
            {
                return Problem("Entity set 'Context.ProductReleasePlans'  is null.");
            }
            var productReleasePlan = await _context.ProductReleasePlans.FindAsync(id);
            if (productReleasePlan != null)
            {
                _context.ProductReleasePlans.Remove(productReleasePlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductReleasePlanExists(int id)
        {
          return _context.ProductReleasePlans.Any(e => e.ProductReleasePlanId == id);
        }

        private IQueryable<ProductReleasePlan> Sort(IQueryable<ProductReleasePlan> productReleasePlans, ProductReleasePlanSort productReleasePlanSort)
        {
            ViewData["PlannedOutputVolume"] = productReleasePlanSort == ProductReleasePlanSort.PlannedOutputVolumeAsc ? ProductReleasePlanSort.PlannedOutputVolumeDesc : ProductReleasePlanSort.PlannedOutputVolumeAsc;
            ViewData["ActualOutputVolume"] = productReleasePlanSort == ProductReleasePlanSort.ActualOutputVolumeAsc ? ProductReleasePlanSort.ActualOutputVolumeDesc : ProductReleasePlanSort.ActualOutputVolumeAsc;
            productReleasePlans = productReleasePlanSort switch
            {
                ProductReleasePlanSort.PlannedOutputVolumeAsc => productReleasePlans.OrderBy(s => s.PlannedOutputVolume),
                ProductReleasePlanSort.PlannedOutputVolumeDesc => productReleasePlans.OrderByDescending(s => s.PlannedOutputVolume),
                ProductReleasePlanSort.ActualOutputVolumeAsc => productReleasePlans.OrderByDescending(s => s.ActualOutputVolume),
                ProductReleasePlanSort.ActualOutputVolumeDesc => productReleasePlans.OrderBy(s => s.ActualOutputVolume),
                _ => productReleasePlans.OrderBy(s => s.CompanyId),
            };
            return productReleasePlans;
        }
        private IQueryable<ProductReleasePlan> Search(IQueryable<ProductReleasePlan> productReleasePlans, string searchCompanyName)
        {
            if (!String.IsNullOrEmpty(searchCompanyName))
            {
                productReleasePlans = productReleasePlans.Where(s => s.Company.Name.Contains(searchCompanyName));
            }
            return productReleasePlans;
        }
    }
}
