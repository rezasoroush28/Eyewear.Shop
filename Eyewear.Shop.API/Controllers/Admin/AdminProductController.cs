using Eyewear.Shop.Application.Commands.Products;
using Eyewear.Shop.Application.Dtos.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eyewear.Shop.API.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    [Authorize(Roles = "Admin")]
    public class AdminProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var res = await _mediator.Send(new CreateProductCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            });

            if(res.IsSuccess) return Ok();
            else return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDto dto)
        {
            await _mediator.Send(new UpdateProductCommand
            {
                ProductId = dto.ProductId,
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            });

            return NoContent();
        }

        //[HttpPost("{productId}/variants")]
        //public async Task<IActionResult> AddVariant(int productId, [FromBody] CreateProductVariantDto dto)
        //{
        //    var id = await _mediator.Send(new CreateProductVariantCommand(productId, dto.Color, dto.Size, dto.Price));
        //    return Ok(new { Id = id });
        //}

        //[HttpPut("variants/{variantId}")]
        //public async Task<IActionResult> UpdateVariant(int variantId, [FromBody] UpdateProductVariantDto dto)
        //{
        //    await _mediator.Send(new UpdateProductVariantCommand(variantId, dto.Color, dto.Size, dto.Price));
        //    return NoContent();
        //}
    }
}
