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
    public class GetAllPagedLineItemsQuery : IRequest<PaginatedResult<GetAllPagedLinItemsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllPagedLineItemsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GGetAllPagedLineItemsQueryHandler : IRequestHandler<GetAllPagedLineItemsQuery, PaginatedResult<GetAllPagedLinItemsResponse>>
    {
        private readonly ILineItemRepository _repository;

        public GGetAllPagedLineItemsQueryHandler(ILineItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllPagedLinItemsResponse>> Handle(GetAllPagedLineItemsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<LineItem, GetAllPagedLinItemsResponse>> expression = e => new GetAllPagedLinItemsResponse
            {
                Id = e.Id,
                //ClaimId = e.ClaimId,
                //CategoryId = e.CategoryId,
                Payee = e.Payee,
                Date = e.Date,
                Description = e.Description,
                Amount = e.Amount,
                //CurrencyCode = e.CurrencyCode,
                USDAmount = e.USDAmount,

            };
            var paginatedList = await _repository.LineItems
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}