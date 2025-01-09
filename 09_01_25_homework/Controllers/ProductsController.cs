using _09_01_25_homework.Interfaces.Services;
using _09_01_25_homework.Model;
using Microsoft.AspNetCore.Mvc;

namespace _09_01_25_homework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var result = await _productService.GetListAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }
           
        }

        [HttpPost("create")]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.AddAsync(product);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetAll), new { id = result.Value.Id }, result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            } 
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, new { Message = result.Error});
            }

        }
    }
}
