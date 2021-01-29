using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace StoreManager.Application.Features.Categories.Commands.Update
{
    public class UpdateCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICategoryRepository _categoryRepository;

            public UpdateProductCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
            {
                _categoryRepository = categoryRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var category = await _categoryRepository.GetByIdAsync(command.Id);

                if (category == null)
                {
                    return Result<int>.Fail($"Category Not Found.");
                }
                else
                {
                    category.Name = command.Name;
                    category.Code = command.Code;
                    await _categoryRepository.UpdateAsync(category);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(category.Id);
                }
            }
        }
    }
}