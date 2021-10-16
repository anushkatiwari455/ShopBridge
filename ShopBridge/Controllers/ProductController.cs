using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBridge.Data.DTO;
using ShopBridge.Data.Entities;
using ShopBridge.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetProducts([FromQuery] string sortBy,[FromQuery]string search,[FromQuery] int pageNumber)
        {
            string logMsg = string.Empty;
            try
            {
                var products = await _productRepository.GetProductsAsync(sortBy, search,pageNumber);
                if(products==null)
                {
                    logMsg = "Get Product Request is null";
                    _logger.LogError(logMsg);
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception e)
            {
                logMsg = "Error: " + e.Message;
                _logger.LogError(logMsg);
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            string logMsg = string.Empty;
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("id is invalid");
                    return BadRequest();
                }
                var product = await _productRepository.GetProductsByIdAsync(id);
                if (product == null)
                {
                    logMsg = "Get ProductById Request is null";
                    _logger.LogError(logMsg);
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception e)
            {
                logMsg = "Error: " + e.Message;
                _logger.LogError(logMsg);
                return BadRequest();
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductDTO product)
        {
            string logMsg = string.Empty;
            try
            {
                if(product==null)
                {
                    _logger.LogError("product details entered are invalid");
                    return BadRequest();
                }
                int id = await _productRepository.AddProductsAsync(product);
                if (id<=0)
                {
                    logMsg = "Product could not be added";
                    _logger.LogError(logMsg);
                    return NotFound();
                }
                return CreatedAtAction(nameof(GetProduct), new { id = id, controller = "Product" }, id);
            }
            catch (Exception e)
            {
                logMsg = "Error: " + e.Message;
                _logger.LogError(logMsg);
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookAsync([FromBody] ProductDTO product, [FromRoute] int id)
        {
            string logMsg = string.Empty;
            try
            {
                if (product == null)
                {
                    _logger.LogError("product details entered are invalid");
                    return BadRequest();
                }
                if (id <= 0)
                {
                    _logger.LogError("id is invalid");
                    return BadRequest();
                }
                if (!await _productRepository.UpdateProductAsync(id, product))
                {
                    logMsg = "Product could not be updated";
                    _logger.LogError(logMsg);
                    return NotFound();
                }
                return Ok(true);
            }
            catch (Exception e)
            {
                logMsg = "Error: " + e.Message;
                _logger.LogError(logMsg);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBookAsync([FromRoute] int id)
        {
            string logMsg = string.Empty;
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("id is invalid");
                    return BadRequest();
                }
                if(!await _productRepository.DeleteProductAsync(id))
                {
                    logMsg = "Product could not be deleted";
                    _logger.LogError(logMsg);
                    return NotFound();
                }
                return Ok(true);
            }
            catch (Exception e)
            {
                logMsg = "Error: " + e.Message;
                _logger.LogError(logMsg);
                return BadRequest();
            }
        }
    }
}
