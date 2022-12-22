using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CourseWorkCompany.Data;
using CourseWorkCompany.Enums;
using CourseWorkCompany.Models;
using X.PagedList;

namespace CourseWorkCompany.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Context _context;

        private IWebHostEnvironment _webHostEnvironment;

        public ProductsController(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult ProductListing(ProductSort productSort, string searchName, string currentFilter, int? page)
        {
            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFilter;
            }
            ViewBag.CurrentFilter = searchName;
            IQueryable<Product> context = _context.Products.Include(p => p.ProductionType).OrderByDescending(i => i.ProductId);
            context = Sort(context, productSort);
            ViewBag.CurrentSort = productSort;
            context = SearchInName(context, searchName);
            int pageNumber = page ?? 1;
            return View(context.ToPagedList(pageNumber, 15));
        }

        public ActionResult ProductUpload(Product product)
        {
            string fileName = null;
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name");
            if (product.ImageFile != null)
            {
                string ImageUploadedFolder = Path.Combine
                    (_webHostEnvironment.WebRootPath, "UploadedImages");

                fileName = Guid.NewGuid().ToString() + "_" +
                    product.ImageFile.FileName;

                string filepath = Path.Combine(ImageUploadedFolder, fileName);

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(fileStream);
                }

                product.ProductPhotoPath = "~/wwwroot/UploadedImages";
                product.ProductFileNamePath = fileName;

                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("ProductListing");
            }

            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> ProductUpdate(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "ProductionTypeId", product.ProductionTypeId);
            return View(product);
        }
        [HttpPost]
        public ActionResult ProductUpdate(Product product)
        {
            string fileName = null;
            ViewData["ProductionTypeId"] = new SelectList(_context.ProductionTypes, "ProductionTypeId", "Name");
            if (product.ImageFile != null)
            {
                string ImageUploadedFolder = Path.Combine
                    (_webHostEnvironment.WebRootPath, "UploadedImages");

                fileName = Guid.NewGuid().ToString() + "_" +
                    product.ImageFile.FileName;

                string filepath = Path.Combine(ImageUploadedFolder, fileName);

                using (var fileStream = new FileStream(filepath, FileMode.CreateNew))
                {
                    product.ImageFile.CopyTo(fileStream);
                }

                product.ProductPhotoPath = "~/wwwroot/UploadedImages";
                product.ProductFileNamePath = fileName;

                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction("ProductListing");
            }

            return View(product);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductionType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductionType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'Context.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductListing");
        }
        private IQueryable<Product> Sort(IQueryable<Product> products, ProductSort productSort)
        {
            ViewData["ProductCharacteristic"] = productSort == ProductSort.CharacteristicAsc ? ProductSort.CharacteristicDesc : ProductSort.CharacteristicAsc;
            ViewData["ProductMeasurementUnit"] = productSort == ProductSort.MeasurementUnitAsc ? ProductSort.MeasurementUnitDesc : ProductSort.MeasurementUnitAsc;
            products = productSort switch
            {
                ProductSort.CharacteristicAsc => products.OrderBy(s => s.Characteristic),
                ProductSort.CharacteristicDesc => products.OrderByDescending(s => s.Characteristic),
                ProductSort.MeasurementUnitAsc => products.OrderByDescending(s => s.MeasurementUnit),
                ProductSort.MeasurementUnitDesc => products.OrderBy(s => s.MeasurementUnit),
                _ => products.OrderBy(s => s.ProductId),
            };
            return products;
        }

        private IQueryable<Product> SearchInName(IQueryable<Product> products, string searchName)
        {
            if (!String.IsNullOrEmpty(searchName))
            {
                products = products.Where(s => s.Name.Contains(searchName));
            }
            return products;
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
