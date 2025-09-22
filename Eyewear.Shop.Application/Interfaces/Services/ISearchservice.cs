using Eyewear.Shop.Application.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces.Services
{
    public interface ISearchservice
    {
        Task<Result<List<ProductSearchResultModel>>> GetSearchResponse(SearchRequestDto searchDto, CancellationToken cancellationToken);
        Task<Result> AddSearchQueryToPopularSearches(string stringQuery, CancellationToken cancellationToken);
        Task<List<string>> GetTopPopularSearchesAsync(CancellationToken cancellationToken, int count = 30);
        Task<Result> IndexAllProduct(List<ProductSearchDto> products, CancellationToken cancellationToken);
        Task<Result> CreateOrUpdateProductDocInIndex(ProductSearchDto productSearchDto, CancellationToken cancellationToken);
        Task<Result> DeleteProductDocFromIndex(int ProductId, CancellationToken cancellationToken);
    }


}
