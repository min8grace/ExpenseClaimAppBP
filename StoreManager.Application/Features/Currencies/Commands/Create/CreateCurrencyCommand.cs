using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Features.Currencies.Commands.Create
{
    public partial class CreateCurrencyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

    }

    public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Result<int>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateCurrencyCommandHandler(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Currency>(request);
            await _currencyRepository.InsertAsync(product);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(product.Id);
        }
    }
}