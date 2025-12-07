using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppia.Models;
using Shoppia.ViewModels;

namespace Shoppia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShoppiaContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ShoppiaContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _context.Products
                .Include(p => p.Category)
                .Take(4)
                .ToListAsync();

            var topVendors = await _context.Vendors
                .Where(v => v.IsActive)
                .Take(3)
                .ToListAsync();

            var vm = new HomeViewModel
            {
                FeaturedProducts = featuredProducts,
                TopVendors = topVendors
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
