using StoreManager.Application.Interfaces.Repositories;
using StoreManager.Domain.Entities.Catalog;
using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
using System;

namespace StoreManager.Application.Features.Claims.Commands.Create
{
    public partial class CreateClaimCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Requester { get; set; }
        public int Approver { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Decimal TotalAmount { get; set; } //= 300.5m;
        public Status Status { get; set; }
        public string RequesterComments { get; set; }
        public string ApproverComments { get; set; }
        public string FinanceComments { get; set; }
    }

    public class CreateClaimCommandHandler : IRequestHandler<CreateClaimCommand, Result<int>>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateClaimCommandHandler(IClaimRepository claimRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Claim>(request);
            await _claimRepository.InsertAsync(product);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(product.Id);
        }
    }
}