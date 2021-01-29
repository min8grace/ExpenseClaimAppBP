using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Currencies.Commands.Update
{
    public class UpdateCurrencyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }


        public class UpdateProductCommandHandler : IRequestHandler<UpdateCurrencyCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICurrencyRepository _currencyRepository;

            public UpdateProductCommandHandler(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork)
            {
                _currencyRepository = currencyRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(UpdateCurrencyCommand command, CancellationToken cancellationToken)
            {
                var currency = await _currencyRepository.GetByIdAsync(command.Id);

                if (currency == null)
                {
                    return Result<int>.Fail($"Currency Not Found.");
                }
                else
                {
                    currency.Name = command.Name;
                    currency.Symbol = command.Symbol;
                    await _currencyRepository.UpdateAsync(currency);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(currency.Id);
                }
            }
        }
    }
}