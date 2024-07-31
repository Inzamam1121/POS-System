using Microsoft.AspNetCore.Mvc;
using POS_System.Services;
using System.Threading.Tasks;
using POSAPIs.Model;

namespace POSAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly SaleTransactionService _saleTransactionService;
        private readonly ILogger<SaleController> _logger;

        public SaleController(SaleTransactionService saleTransactionService, ILogger<SaleController> logger)
        {
            _saleTransactionService = saleTransactionService;
            _logger = logger;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSale([FromHeader] string Authorization)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _saleTransactionService.IsCashierAsync(token);
            _logger.LogInformation("Requesting Starting Sale...");
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can start a sale." });
            }

            var result = await _saleTransactionService.StartNewSaleAsync(token);
            if (result)
            {
                return Ok(new { message = "Sale started successfully" });
            }
            return BadRequest(new { message = "Unable to start sale" });
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProductToSale([FromHeader] string Authorization, [FromBody] SaleProductModel model)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _saleTransactionService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can add products to a sale." });
            }

            var result = await _saleTransactionService.AddProductToSaleAsync(model.ProductId, model.Quantity, token);
            if (result)
            {
                return Ok(new { message = "Product added to sale successfully" });
            }
            return BadRequest(new { message = "Unable to add product to sale" });
        }

        [HttpPost("removeproduct")]
        public async Task<IActionResult> RemoveProductFromSale([FromHeader] string Authorization, [FromBody] SaleProductModel model)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _saleTransactionService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can remove products from a sale." });
            }

            var result = await _saleTransactionService.RemoveProductFromSaleAsync(model.ProductId, model.Quantity, token);
            if (result)
            {
                return Ok(new { message = "Product removed from sale successfully" });
            }
            return BadRequest(new { message = "Unable to remove product from sale" });
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteSale([FromHeader] string Authorization)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _saleTransactionService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can complete a sale." });
            }

            var result = await _saleTransactionService.CompleteSaleAsync(token);
            if (result)
            {
                return Ok(new { message = "Sale completed successfully" });
            }
            return BadRequest(new { message = "Unable to complete sale" });
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentSale([FromHeader] string Authorization)
        {
            var token = Authorization.Split(' ')[1];
            var isCashier = await _saleTransactionService.IsCashierAsync(token);
            if (!isCashier)
            {
                return Unauthorized(new { message = "Only cashiers can view the current sale." });
            }

            var sale = _saleTransactionService.GetCurrentSale();
            if (sale != null)
            {
                return Ok(sale);
            }
            return NotFound(new { message = "No sale in progress" });
        }
    }
}
