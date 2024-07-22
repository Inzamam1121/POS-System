using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_System.Data;
using POS_System.Entities;
using POS_System.Services;
using System.Threading.Tasks;

namespace POSAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly DataContextEntity _context;

        public ProductController(ProductService productService, UserService userService, DataContextEntity context)
        {
            _productService = productService;
            _userService = userService;
            _context = context;
        }

        [HttpPost("seedData")]
        public IActionResult SeedData()
        {
            var admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
            _userService.RegisterUser(admin);

            var cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
            _userService.RegisterUser(cashier);

            var electronicsCategory = new Category { Name = "Electronics" };
            _context.Categories.Add(electronicsCategory);
            _context.SaveChanges();

            return Ok("Data seeded successfully.");
        }

        [HttpGet("getCategories")]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }

            var categoryExists = _context.Categories.Any(c => c.Id == product.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("The specified category does not exist.");
            }

            var loggedInUser = _userService.AuthenticateUser("admin@gmail.com", "admin");

            bool result = await _productService.AddProduct(product, loggedInUser);
            if (result)
            {
                return Ok("Product added successfully.");
            }
            else
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("getProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .ToList();
            return Ok(products);
        }


        [HttpPut("updateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product product)
        {
            if (product == null || productId != product.Id)
            {
                return BadRequest("Invalid product data.");
            }

            var loggedInUser = _userService.AuthenticateUser("admin@gmail.com", "admin");

            bool result = await _productService.UpdateProduct(product, loggedInUser);
            if (result)
            {
                return Ok("Product updated successfully.");
            }
            else
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("removeProduct/{productId}")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var loggedInUser = _userService.AuthenticateUser("admin@gmail.com", "admin");

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            await _productService.RemoveProduct(product, loggedInUser);
            return Ok("Product removed successfully.");
        }
    }
}
