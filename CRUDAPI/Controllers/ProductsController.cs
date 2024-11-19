using CRUDAPI.Data;
using CRUDAPI.DTOs.Departments;
using CRUDAPI.DTOs;
using CRUDAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRUDAPI.DTOs.Products;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        private readonly ILogger<ProductsController> logger;
        public ProductsController(ApplicationDBContext context, ILogger<ProductsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await context.Products.Select(
                x => new GetProductsDto()
            {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description,
            }).ToListAsync();
            return Ok(products);
        }
        [HttpGet("details")]
        public async Task<IActionResult> GetByID(int id)
        {
            var prod = await context.Products.FindAsync(id);
            if (prod is null)
                return NotFound();
            var response = new GetByIDProductDto()
            {
                Id = prod.Id,
                Name = prod.Name,
                Price = prod.Price,
                Description = prod.Description,
            };
            return Ok(response);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateProductDto prodDto)
        {
            Product prod = new Product()
            {
                Name = prodDto.Name,
                Price = prodDto.Price,
                Description = prodDto.Description,
            };
            await context.Products.AddAsync(prod);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, UpdateProductDto prodDto)
        {
            var entity = await context.Products.FindAsync(id);
            if (entity is null)
                return NotFound("Product Not Found");

            entity.Name = prodDto.Name;
            entity.Price = prodDto.Price;
            entity.Description = prodDto.Description;

            await context.SaveChangesAsync();

            return Ok(prodDto);
        }
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            var entity = await context.Products.FindAsync(id);
            if (entity is null)
                return NotFound("Product Not Found");

            context.Products.Remove(entity);
            var res = new RemoveProductDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
            };
            await context.SaveChangesAsync();

            return Ok(res);
        }

    }
}
