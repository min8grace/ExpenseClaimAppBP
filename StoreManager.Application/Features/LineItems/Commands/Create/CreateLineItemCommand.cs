using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
using System;

namespace StoreManager.Application.Features.LineItems.Commands.Create
{
    public partial class CreateLineItemCommand : IRequest<Result<int>>
    {
        public int ClaimId { get; set; }
        public int CategoryId { get; set; }
        public string Payee { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public Decimal USDAmount { get; set; }
        public byte[] Receipt { get; set; }

    }

    public class CreateLineItemCommandHandler : IRequestHandler<CreateLineItemCommand, Result<int>>
    {
        private readonly ILineItemRepository _lineItemRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateLineItemCommandHandler(ILineItemRepository lineItemRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _lineItemRepository = lineItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateLineItemCommand request, CancellationToken cancellationToken)
        {
            var lineItem = _mapper.Map<LineItem>(request);
            await _lineItemRepository.InsertAsync(lineItem);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(lineItem.Id);
        }
    }
}