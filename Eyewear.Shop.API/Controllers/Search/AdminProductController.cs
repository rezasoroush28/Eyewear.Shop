using Eyewear.Shop.API.Models.Search;
using Eyewear.Shop.Application.Commands.Products;
using Eyewear.Shop.Application.Dtos.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eyewear.Shop.API.Controllers
{
    [ApiController]
    [Route("api/SearchProducts")]
    [Authorize(Roles = "Admin")]
    public class SearchProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Search([FromBody] SearchProductRequestModel searchProductRequestModel)
        {

        }


    }
}

