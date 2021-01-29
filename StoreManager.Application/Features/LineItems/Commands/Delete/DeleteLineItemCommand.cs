using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.LineItems.Commands.Delete
{
    public class DeleteLineItemCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteLineItemCommandHandler : IRequestHandler<DeleteLineItemCommand, Result<int>>
        {
            private readonly ILineItemRepository _lineItemRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteLineItemCommandHandler(ILineItemRepository lineItemRepository, IUnitOfWork unitOfWork)
            {
                _lineItemRepository = lineItemRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteLineItemCommand command, CancellationToken cancellationToken)
            {
                var lineItem = await _lineItemRepository.GetByIdAsync(command.Id);
                await _lineItemRepository.DeleteAsync(lineItem);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(lineItem.Id);
            }
        }
    }
}