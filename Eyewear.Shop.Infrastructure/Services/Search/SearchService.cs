using Eyewear.Shop.Application.Interfaces;
using Eyewear.Shop.Application.Models.Search;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eyewear.Shop.Infrastructure.Services.Search
{
    public class SearchService : ISearchservice
    {
        private readonly IElasticClient _elasticClient;

        public SearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<Result<List<ProductSearchResultModel>>> GetSearchResponse(SearchRequestDto searchDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(searchDto.Query))
                return new();

            var searchResponse = await _elasticClient.SearchAsync<ProductSearchDto>(s => s
                .Index("Products")
                .Query(q =>
                {
                    // If category is provided in the request
                    if (!string.IsNullOrWhiteSpace(searchDto.Category))
                    {
                        return q.Bool(b => b
                            .Must(
                                m => m.MultiMatch(mm => mm
                                    .Query(searchDto.Query)
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
                            .Query(searchDto.Query)
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
    }

    public class ProductSearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ParentCategory { get; set; }
        public List<KeyValuePair<string, string>> Attributes { get; set; } = new();
    }
}
