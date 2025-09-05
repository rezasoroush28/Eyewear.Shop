using Eyewear.Shop.Application.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces
{
    public interface ISearchservice 
    {
        Task<Result<List<ProductSearchResultModel>>> GetSearchResponse(SearchRequestDto searchDto, CancellationToken cancellationToken);
    }


}
