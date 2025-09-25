using Eyewear.Shop.API.Models.Search;
using Eyewear.Shop.Application.Commands.Products;
using Eyewear.Shop.Application.Commands.Search;
using Eyewear.Shop.Application.Dtos.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eyewear.Shop.API.Controllers
{
    [ApiController]
    [Route("api/SearchProducts")]
    public class SearchProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Search([FromBody] SearchProductRequestModel searchProductRequestModel)
        {
            var command = new SearchProductCommand
            {
                StringQuery = searchProductRequestModel.StringQuery,
                //MaxPrice = searchProductRequestModel.MaxPrice,
                //MinPrice = searchProductRequestModel.MinPrice,
                SortBy = searchProductRequestModel.SortBy,
                Page = searchProductRequestModel.Page,
                PageSize = searchProductRequestModel.PageSize
            };

            if (searchProductRequestModel.CategoryId != null)
            {
                command.CategoryString = searchProductRequestModel.CategoryString;
            }

            var res = await _mediator.Send(command);
            if (!res.IsSuccess)
            {
                return BadRequest(res.ErrorMessage);
            }

            return Ok(res.Data);


        }

        public async Task<IActionResult> GetSearchResults([FromBody] SearchProductRequestModel searchProductRequestModel)
        {
            var command = new GetProductSearchResultsCommand
            {
                StringQuery = searchProductRequestModel.StringQuery,
                //MaxPrice = searchProductRequestModel.MaxPrice,
                //MinPrice = searchProductRequestModel.MinPrice,
                SortBy = searchProductRequestModel.SortBy,
                Page = searchProductRequestModel.Page,
                PageSize = searchProductRequestModel.PageSize
            };

            if (searchProductRequestModel.CategoryId != null)
            {
                command.CategoryString = searchProductRequestModel.CategoryString;
            }

            var res = await _mediator.Send(command);
            if (!res.IsSuccess)
            {
                return BadRequest(res.ErrorMessage);
            }

            return Ok(res.Data);


        }


    }

}

