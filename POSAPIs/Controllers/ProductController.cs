using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_System.Entities;
using POS_System.Services;
using System.Threading.Tasks;
using POSAPIs.Model;

namespace POSAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel productmodel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _productService.GetUserFromTokenAsync(token);

            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            try
            {
                Product product = new Product(productmodel.name, productmodel.price, productmodel.quantity, productmodel.type, productmodel.categoryId);

                bool result = await _productService.AddProductAsync(product, token);
                if (result)
                {
                    return Ok(new { message = "Product added successfully." });
                }
                return Unauthorized(new { message = "Not authorized to add product." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel productmodel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _productService.GetUserFromTokenAsync(token);
            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            Product product = new Product(productmodel.productid,productmodel.name, productmodel.price, productmodel.quantity, productmodel.type, productmodel.categoryId);

            var success = await _productService.UpdateProductAsync(product, token);
            if (!success)
            {
                return BadRequest(new { message = "Failed to update product." });
            }
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _productService.GetUserFromTokenAsync(token);
            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _productService.DeleteProductAsync(id, token);
            if (!success)
            {
                return BadRequest(new { message = "Failed to delete product." });
            }
            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _productService.GetUserFromTokenAsync(token);
            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            return Ok(product);
        }

        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await _productService.GetUserFromTokenAsync(token);
            if (admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }


        [HttpPut("updatequantity")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _productService.GetUserFromTokenAsync(token);
            if (user == null || (user.UserRole != UserRole.Admin && user.UserRole != UserRole.Cashier))
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _productService.UpdateProductQuantityAsync(model.ProductId, model.Quantity, token);
            if (!success)
            {
                return NotFound(new { message = "Product not found or you are not authorized to update the quantity." });
            }
            return Ok(new { message = "Product quantity updated successfully" });
        }
    }
}
