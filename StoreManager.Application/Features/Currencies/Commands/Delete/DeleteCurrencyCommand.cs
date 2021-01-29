using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Currencies.Commands.Delete
{
    public class DeleteCurrencyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, Result<int>>
        {
            private readonly ICurrencyRepository _currencyRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork)
            {
                _currencyRepository = currencyRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteCurrencyCommand command, CancellationToken cancellationToken)
            {
                var product = await _currencyRepository.GetByIdAsync(command.Id);
                await _currencyRepository.DeleteAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(product.Id);
            }
        }
    }
}