using StoreManager.Application.Exceptions;
using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.LineItems.Commands.Update
{
    public class UpdateLineItemImageCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public byte[] Receipt { get; set; }//image


        public class UpdateLineItemImageCommandHandler : IRequestHandler<UpdateLineItemImageCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILineItemRepository _lineItemRepository;

            public UpdateLineItemImageCommandHandler(ILineItemRepository lineItemRepository, IUnitOfWork unitOfWork)
            {
                _lineItemRepository = lineItemRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(UpdateLineItemImageCommand command, CancellationToken cancellationToken)
            {
                var lineItem = await _lineItemRepository.GetByIdAsync(command.Id);

                if (lineItem == null)
                {
                    throw new ApiException($"LineItem Not Found.");
                }
                else
                {
                    lineItem.Receipt = command.Receipt;
                    await _lineItemRepository.UpdateAsync(lineItem);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(lineItem.Id);
                }
            }
        }
    }
}