using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Features.LineItems.Commands.Update
{
    public class UpdateLineItemCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public int CategoryId { get; set; }
        public string Payee { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public Decimal USDAmount { get; set; }

        public class UpdateLineItemCommandHandler : IRequestHandler<UpdateLineItemCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILineItemRepository _lineItemRepository;

            public UpdateLineItemCommandHandler(ILineItemRepository lineItemRepository, IUnitOfWork unitOfWork)
            {
                _lineItemRepository = lineItemRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(UpdateLineItemCommand command, CancellationToken cancellationToken)
            {
                var lineItem = await _lineItemRepository.GetByIdAsync(command.Id);

                if (lineItem == null)
                {
                    return Result<int>.Fail($"LineItem Not Found.");
                }
                else
                {
                    lineItem.ClaimId = command.ClaimId;
                    lineItem.CategoryId = command.CategoryId;
                    lineItem.Payee = command.Payee;
                    lineItem.Date = command.Date;
                    lineItem.Description = command.Description;
                    lineItem.Amount = command.Amount;
                    lineItem.CurrencyCode = command.CurrencyCode;
                    lineItem.USDAmount = command.USDAmount;

                    await _lineItemRepository.UpdateAsync(lineItem);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(lineItem.Id);
                }
            }
        }
    }
}