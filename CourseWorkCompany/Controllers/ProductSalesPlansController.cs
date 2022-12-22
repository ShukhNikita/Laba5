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

namespace CourseWorkCompany.Controllers
{
    public class ProductSalesPlansController : Controller
    {
        private readonly Context _context;

        public ProductSalesPlansController(Context context)
        {
            _context = context;
        }

        // GET: ProductSalesPlans
        public async Task<IActionResult> Index()
        {
            var context = _context.ProductSalesPlans.Include(p => p.Company).Include(p => p.ProductionType);
            return View(await context.ToListAsync());
        }

        // GET: ProductSalesPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductSalesPlans == null)
            {
                return NotFound();
            }

            var productSalesPlan = await _context.ProductSalesPlans
                .Include(p => p.Company)
                .Include(p => p.ProductionType)
                .FirstOrDefaultAsync(m => m.ProductSalesPlanId == id);
            if (productSalesPlan == null)
            {
                return NotFound();
            }

            return View(productSalesPlan);
        }

        // GET: ProductSalesPlans/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name");
            return View();
        }

        // POST: ProductSalesPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductSalesPlanId,CompanyId,ProductionTypeId,PlannedImplementationVolume,ActualImplementationVolume,QuarterInfo,YearInfo")] ProductSalesPlan productSalesPlan)
        {
            if (ModelState != null)
            {
                _context.Add(productSalesPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", productSalesPlan.CompanyId);
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name", productSalesPlan.ProductionTypeId);
            return View(productSalesPlan);
        }

        // GET: ProductSalesPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductSalesPlans == null)
            {
                return NotFound();
            }

            var productSalesPlan = await _context.ProductSalesPlans.FindAsync(id);
            if (productSalesPlan == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", productSalesPlan.CompanyId);
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name", productSalesPlan.ProductionTypeId);
            return View(productSalesPlan);
        }

        // POST: ProductSalesPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductSalesPlanId,CompanyId,ProductionTypeId,PlannedImplementationVolume,ActualImplementationVolume,QuarterInfo,YearInfo")] ProductSalesPlan productSalesPlan)
        {
            if (id != productSalesPlan.ProductSalesPlanId)
            {
                return NotFound();
            }

            if (ModelState != null)
            {
                try
                {
                    _context.Update(productSalesPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSalesPlanExists(productSalesPlan.ProductSalesPlanId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", productSalesPlan.CompanyId);
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name", productSalesPlan.ProductionTypeId);
            return View(productSalesPlan);
        }

        // GET: ProductSalesPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductSalesPlans == null)
            {
                return NotFound();
            }

            var productSalesPlan = await _context.ProductSalesPlans
                .Include(p => p.Company)
                .Include(p => p.ProductionType)
                .FirstOrDefaultAsync(m => m.ProductSalesPlanId == id);
            if (productSalesPlan == null)
            {
                return NotFound();
            }

            return View(productSalesPlan);
        }

        // POST: ProductSalesPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductSalesPlans == null)
            {
                return Problem("Entity set 'Context.ProductSalesPlans'  is null.");
            }
            var productSalesPlan = await _context.ProductSalesPlans.FindAsync(id);
            if (productSalesPlan != null)
            {
                _context.ProductSalesPlans.Remove(productSalesPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSalesPlanExists(int id)
        {
          return _context.ProductSalesPlans.Any(e => e.ProductSalesPlanId == id);
        }
    }
}
