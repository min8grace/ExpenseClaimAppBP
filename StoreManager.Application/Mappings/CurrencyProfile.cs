using AutoMapper;
using StoreManager.Application.Features.Currencies.Commands.Create;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Application.Features.Currencies.Queries.GetById;
using StoreManager.Domain.Entities.Expense;

namespace StoreManager.Application.Mappings
{
    internal class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CreateCurrencyCommand, Currency>().ReverseMap();
            CreateMap<GetCurrencyByIdResponse, Currency>().ReverseMap();
            CreateMap<GetAllCurrenciesResponse, Currency>().ReverseMap();
        }
    }
}