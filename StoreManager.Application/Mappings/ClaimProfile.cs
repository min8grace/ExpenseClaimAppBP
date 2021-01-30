using StoreManager.Application.Features.Claims.Commands.Create;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Application.Features.Claims.Queries.GetById;
using StoreManager.Domain.Entities.Catalog;
using AutoMapper;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Mappings
{
    internal class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<CreateClaimCommand, Claim>().ReverseMap();
            CreateMap<GetClaimByIdResponse, Claim>().ReverseMap();
            CreateMap<GetAllClaimsResponse, Claim>().ReverseMap();
        }
    }
}
