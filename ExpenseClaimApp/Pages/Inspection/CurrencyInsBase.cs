﻿using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages.Inspection
{
    public class CurrencyInsBase : ComponentBase
    {

        [Inject]
        public ICurrencyService CurrencyService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

        public List<GetAllCurrenciesResponse> Currencies { get; set; } //= new List<GetAllCurrenciesResponse>();
        public int Id { get; set; }
        public StoreManager.Domain.Entities.Expense.Currency Currency { get; set; } = new StoreManager.Domain.Entities.Expense.Currency();
        public CurrencyEditModel CurrencyEditModel { get; set; } = new CurrencyEditModel();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        protected bool CreateEditMode { get; set; } = false;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CreateEditMode = false;
            CurrencyEditModel = new CurrencyEditModel();
            Currencies = (await CurrencyService.GetCurrencies()).ToList();
        }
        protected async Task Edit_Click(int InputId, int b)
        {
            CreateEditMode = true;
            Currency = await CurrencyService.GetCurrencyById(InputId);
            Mapper.Map(Currency, CurrencyEditModel);
        }
        protected async Task Delete_Click(int InputId)
        {
            await CurrencyService.DeleteCurrency(InputId);
            NavigationManager.NavigateTo("/ins/Currency", true);
        }
        protected async Task Create_Click()
        {
            CreateEditMode = true;
            CurrencyEditModel = new CurrencyEditModel();

        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(CurrencyEditModel, Currency);

            if (Currency.Id != 0)//Edit
            {
                await CurrencyService.UpdateCurrency(Currency);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
                //StateHasChanged();
                NavigationManager.NavigateTo("/ins/Currency", true);

            }
            else//Create
            {
                StoreManager.Domain.Entities.Expense.Currency result = null;
                result = await CurrencyService.CreateCurrency(Currency);
                if (result != null)
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong Creating the new employee. Please try again.";
                    Saved = false;
                }
                NavigationManager.NavigateTo("/ins/Currency", true);
            }

        }
    }
}
