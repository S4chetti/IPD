namespace IPD.Data
{
    using IPD.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Define your DbSets (Tables) here
    public DbSet<Product> Products { get; set; }
        public class ProductsController : Controller
        {
            private readonly ApplicationDbContext _context;

            public ProductsController(ApplicationDbContext context)
            {
                _context = context;
            }

            public IActionResult Index()
            {
                var products = _context.Products.ToList();
                return View(products);
            }
        }
    }
}
