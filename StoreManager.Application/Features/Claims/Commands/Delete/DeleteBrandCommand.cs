using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
namespace StoreManager.Application.Features.Claims.Commands.Delete
{
    public class DeleteClaimCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, Result<int>>
        {
            private readonly IClaimRepository _claimRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteClaimCommandHandler(IClaimRepository claimRepository, IUnitOfWork unitOfWork)
            {
                _claimRepository = claimRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteClaimCommand command, CancellationToken cancellationToken)
            {
                var product = await _claimRepository.GetByIdAsync(command.Id);
                await _claimRepository.DeleteAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(product.Id);
            }
        }
    }
}