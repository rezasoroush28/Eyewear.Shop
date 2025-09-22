using Eyewear.Shop.Application.Interfaces.Services;
using Eyewear.Shop.Application.Models.Search;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Eyewear.Shop.Infrastructure.Services.Search
{
    public class SearchService : ISearchservice
    {
        private readonly IElasticClient _elasticClient;
        private const string ProductIndex = "products";
        public SearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<Result> AddSearchQueryToPopularSearches(string stringQuery, CancellationToken cancellationToken)
        {
            string normalizedQuery = stringQuery.Trim().ToLowerInvariant();
            var indexName = "popular-searches";

            var existingSearchQuery = await _elasticClient.GetAsync<PopularSearchDto>(normalizedQuery, idx => idx.Index(indexName), cancellationToken);
            if (existingSearchQuery.Found && existingSearchQuery.Source != null)
            {
                var updated = existingSearchQuery.Source;
                updated.Count += 1;
                updated.LastSearchedAt = DateTime.UtcNow;

                await _elasticClient.IndexAsync(updated, i => i.Index(indexName).Id(normalizedQuery), cancellationToken);
            }
            else
            {
                var newEntry = new PopularSearchDto
                {
                    Id = normalizedQuery,
                    Query = stringQuery,
                    Count = 1,
                    LastSearchedAt = DateTime.UtcNow
                };

                await _elasticClient.IndexAsync(newEntry, i => i.Index(indexName).Id(normalizedQuery), cancellationToken);
            }


            return Result.Success();

        }
        public async Task<Result<List<ProductSearchResultModel>>> GetSearchResponse(SearchRequestDto searchDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(searchDto.StringQuery))
                return new();

            var searchResponse = await _elasticClient.SearchAsync<ProductSearchDto>(s => s
                .Index(ProductIndex)
                .Query(q =>
                {
                    // If category is provided in the request
                    if (!string.IsNullOrWhiteSpace(searchDto.Category))
                    {
                        return q.Bool(b => b
                            .Must(
                                m => m.MultiMatch(mm => mm
                                    .Query(searchDto.StringQuery)
                                    .Fields(f => f
                                        .Field(p => p.Name, 2.0)
                                        .Field(p => p.Category, 1.8)
                                        .Field(p => p.ParentCategory, 1.6)
                                        .Field(p => p.Attributes.Select(a => a.Value), 1.4)
                                        .Field(p => p.Description, 1.0)
                                    )
                                    .Type(TextQueryType.BestFields)
                                    .Fuzziness(Fuzziness.Auto)
                                ),
                                m => m.Bool(bb => bb
                                    .Should(
                                        s => s.Match(mq => mq
                                            .Field(p => p.Category)
                                            .Query(searchDto.Category)
                                        ),
                                        s => s.Match(mq => mq
                                            .Field(p => p.ParentCategory)
                                            .Query(searchDto.Category)
                                        )
                                    )
                                    .MinimumShouldMatch(1)
                                )
                            )
                        );
                    }
                    else
                    {
                        // Default query (no category filtering)
                        return q.MultiMatch(mm => mm
                            .Query(searchDto.StringQuery)
                            .Fields(f => f
                                .Field(p => p.Name, 2.0)
                                .Field(p => p.Category, 1.8)
                                .Field(p => p.ParentCategory, 1.6)
                                .Field(p => p.Attributes.Select(a => a.Value), 1.4)
                                .Field(p => p.Description, 1.0)
                            )
                            .Type(TextQueryType.BestFields)
                            .Fuzziness(Fuzziness.Auto)
                        );
                    }
                })
                .Sort(srt =>
                {
                    if (searchDto.SortBy == "relevance" || string.IsNullOrWhiteSpace(searchDto.SortBy))
                        return srt.Descending(SortSpecialField.Score);

                    return srt.Descending(p => p.Id); // fallback
                })
                .From((searchDto.Page - 1) * searchDto.PageSize)
                .Size(searchDto.PageSize),
                cancellationToken
            );

            var result = searchResponse.Hits
                .Select(h => new ProductSearchResultModel
                {
                    Id = h.Source.Id,
                    Name = h.Source.Name
                })
                .ToList();

            return Result<List<ProductSearchResultModel>>.Success(result);
        }
        public async Task<List<string>> GetTopPopularSearchesAsync(CancellationToken cancellationToken, int count = 30)
        {
            var response = await _elasticClient.SearchAsync<PopularSearchDto>(s => s
                .Index("popular-searches")
                .Size(count)
                .Sort(ss => ss.Descending(p => p.Count))
                .Query(q => q.MatchAll()), cancellationToken);

            if (!response.IsValid || response.Documents.Count == 0)
                return new List<string>();

            return response.Documents
                .Select(d => d.Query)
                .Where(q => !string.IsNullOrWhiteSpace(q))
                .Take(count)
                .ToList();
        }
        public async Task<Result> IndexAllProduct(List<ProductSearchDto> products, CancellationToken cancellationToken)
        {
            var existsResponse = await _elasticClient.Indices.ExistsAsync(ProductIndex, ct: cancellationToken);
            if (existsResponse.Exists)
            {
                await _elasticClient.Indices.DeleteAsync(ProductIndex, ct: cancellationToken);
            }

            var createResponse = await _elasticClient.Indices.CreateAsync(ProductIndex, c => c
             .Map<ProductSearchDto>(m => m
                 .AutoMap()
                 .Properties(ps => ps
                     .Text(t => t.Name(p => p.Id.ToString()))
                     .Text(t => t.Name(p => p.Name).Boost(2))
                     .Text(t => t.Name(p => p.Category))
                     .Text(t => t.Name(p => p.ParentCategory))
                     .Text(t => t.Name(p => p.Description))
                     .Text (t => t.Name(p => p.Attributes))
                 )
             ),
             ct: cancellationToken);

            if (!createResponse.IsValid)
                return Result.Failure("Failed to create Elasticsearch index");

            var bulkResponse = await _elasticClient.BulkAsync(b => b
           .Index(ProductIndex)
           .IndexMany(products),
           cancellationToken
       );

            if (bulkResponse.Errors)
                return Result.Failure("Bulk indexing failed: some documents were not indexed");

            return Result.Success();
        }
        public async Task<Result> CreateOrUpdateProductDocInIndex(ProductSearchDto productSearchDto, CancellationToken cancellationToken)
        {
            var response = await _elasticClient.IndexAsync(ProductIndex, c => c
            .Index(ProductIndex)
            .Id(productSearchDto.Id)
            .Refresh(Elasticsearch.Net.Refresh.True),
            cancellationToken);

            if(!response.IsValid)
                return Result.Failure("Failed to index or update product");

            return Result.Success();
        }
        public async Task<Result> DeleteProductDocFromIndex(int productId, CancellationToken cancellationToken)
        {
            var response = await _elasticClient.DeleteAsync<ProductSearchDto>(productId, d => d
            .Index(ProductIndex), cancellationToken);

            if(!response.IsValid)
                return Result.Failure("Failed to delete product from index");

            return Result.Success();

        }
    }



    public class PopularSearchDto
    {
        public string Id { get; set; } = default!;
        public string Query { get; set; } = default!;
        public int Count { get; set; } = 1;
        public DateTime LastSearchedAt { get; set; } = DateTime.UtcNow;
    }
}
