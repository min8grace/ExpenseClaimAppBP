using AutoMapper;
using StoreManager.Application.Features.Categories.Commands.Create;
using StoreManager.Application.Features.Categories.Commands.Update;
using StoreManager.Application.Features.Claims.Commands.Create;
using StoreManager.Application.Features.Claims.Commands.Update;
using StoreManager.Application.Features.Claims.Queries.GetById;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Models
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {

            CreateMap<Claim, ClaimEditModel>();
            CreateMap<ClaimEditModel, Claim>();
            CreateMap<GetClaimByIdResponse, Claim>();
            CreateMap<Claim, UpdateClaimCommand>();
            CreateMap<Claim, CreateClaimCommand>();
        }
    }
}
