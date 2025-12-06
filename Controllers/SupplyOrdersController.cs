using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoppia.Models;

namespace Shoppia.Controllers
{
    [Authorize]
    public class SupplyOrdersController : Controller
    {
        private readonly ShoppiaContext _context;

        public SupplyOrdersController(ShoppiaContext context)
        {
            _context = context;
        }

        // GET: SupplyOrders
        public async Task<IActionResult> Index()
        {
            var supplyOrders = await _context.SupplyOrders
                .Include(s => s.Vendor)
                .ToListAsync();
            return View(supplyOrders);
        }

        // GET: SupplyOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyOrder = await _context.SupplyOrders
                .Include(s => s.Vendor)
                .Include(s => s.SupplyOrderDetails)
                    .ThenInclude(sod => sod.Product)
                .FirstOrDefaultAsync(m => m.SupplyOrderId == id);
            if (supplyOrder == null)
            {
                return NotFound();
            }

            return View(supplyOrder);
        }

        // GET: SupplyOrders/Create
        public IActionResult Create()
        {
            ViewData["VendorId"] = new SelectList(_context.Vendors, "VendorId", "Name");
            return View();
        }

        // POST: SupplyOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplyOrderId,Total,SupplyDate,VendorId")] SupplyOrder supplyOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplyOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VendorId"] = new SelectList(_context.Vendors, "VendorId", "Name", supplyOrder.VendorId);
            return View(supplyOrder);
        }

        // GET: SupplyOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyOrder = await _context.SupplyOrders.FindAsync(id);
            if (supplyOrder == null)
            {
                return NotFound();
            }
            ViewData["VendorId"] = new SelectList(_context.Vendors, "VendorId", "Name", supplyOrder.VendorId);
            return View(supplyOrder);
        }

        // POST: SupplyOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplyOrderId,Total,SupplyDate,VendorId")] SupplyOrder supplyOrder)
        {
            if (id != supplyOrder.SupplyOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplyOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyOrderExists(supplyOrder.SupplyOrderId))
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
            ViewData["VendorId"] = new SelectList(_context.Vendors, "VendorId", "Name", supplyOrder.VendorId);
            return View(supplyOrder);
        }

        // GET: SupplyOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplyOrder = await _context.SupplyOrders
                .Include(s => s.Vendor)
                .FirstOrDefaultAsync(m => m.SupplyOrderId == id);
            if (supplyOrder == null)
            {
                return NotFound();
            }

            return View(supplyOrder);
        }

        // POST: SupplyOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplyOrder = await _context.SupplyOrders.FindAsync(id);
            if (supplyOrder != null)
            {
                _context.SupplyOrders.Remove(supplyOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyOrderExists(int id)
        {
            return _context.SupplyOrders.Any(e => e.SupplyOrderId == id);
        }
    }
}
