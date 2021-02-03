using AutoMapper;
using StoreManager.Application.Features.Currencies.Commands.Create;
using StoreManager.Application.Features.Currencies.Commands.Update;
using StoreManager.Domain.Entities.Expense;


namespace ExpenseClaimApp.Models
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {

            CreateMap<Currency, CurrencyEditModel>();
            CreateMap<CurrencyEditModel, Currency>();
            CreateMap<Currency, UpdateCurrencyCommand>();
            CreateMap<Currency, CreateCurrencyCommand>();
        }
    }
}
