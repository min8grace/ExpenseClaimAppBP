using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Features.Categories.Commands.Create
{
    public partial class CreateCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<int>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Category>(request);
            await _categoryRepository.InsertAsync(product);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(product.Id);
        }
    }
}