using StoreManager.Application.Extensions;
using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Features.LineItems.Queries.GetAllPaged
{
    public class GetAllLineItemsQuery : IRequest<PaginatedResult<GetAllLineItemsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllLineItemsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GGetAllLineItemsQueryHandler : IRequestHandler<GetAllLineItemsQuery, PaginatedResult<GetAllLineItemsResponse>>
    {
        private readonly ILineItemRepository _repository;

        public GGetAllLineItemsQueryHandler(ILineItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllLineItemsResponse>> Handle(GetAllLineItemsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<LineItem, GetAllLineItemsResponse>> expression = e => new GetAllLineItemsResponse
            {
                Id = e.Id,
                ClaimId = e.ClaimId,
                CategoryId = e.CategoryId,
                Payee = e.Payee,
                Date = e.Date,
                Description = e.Description,
                Amount = e.Amount,
                CurrencyCode = e.CurrencyCode,
                USDAmount = e.USDAmount,

            };
            var paginatedList = await _repository.LineItems
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}