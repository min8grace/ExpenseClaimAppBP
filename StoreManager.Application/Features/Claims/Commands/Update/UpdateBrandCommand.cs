using StoreManager.Application.Interfaces.Repositories;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using StoreManager.Domain.Entities.Expense;
using System;

namespace StoreManager.Application.Features.Claims.Commands.Update
{
    public class UpdateClaimCommand : IRequest<Result<int>>
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

        public class UpdateProductCommandHandler : IRequestHandler<UpdateClaimCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IClaimRepository _claimRepository;

            public UpdateProductCommandHandler(IClaimRepository claimRepository, IUnitOfWork unitOfWork)
            {
                _claimRepository = claimRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(UpdateClaimCommand command, CancellationToken cancellationToken)
            {
                var claim = await _claimRepository.GetByIdAsync(command.Id);

                if (claim == null)
                {
                    return Result<int>.Fail($"Claim Not Found.");
                }
                else
                {
                    claim.Title = command.Title;
                    claim.Requester = command.Requester;
                    claim.Approver = command.Approver;
                    claim.SubmitDate = command.SubmitDate;
                    claim.ApprovalDate = command.ApprovalDate;
                    claim.ProcessedDate = command.ProcessedDate;
                    claim.TotalAmount = command.TotalAmount;
                    claim.Status = command.Status;
                    //claim.Status = (Status)Enum.Parse(typeof(Status), command.Status);
                    claim.RequesterComments = command.RequesterComments;
                    claim.ApproverComments = command.ApproverComments;
                    claim.FinanceComments = command.FinanceComments;

                    await _claimRepository.UpdateAsync(claim);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(claim.Id);
                }
            }
        }
    }
}