using AutoMapper;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Currencies.Commands.Update;
using StoreManager.Application.Features.Currencies.Queries.GetAllCurrencies;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        public CurrencyService(HttpClient httpClient, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.mapper = mapper;
        }
        private const int apiversion = 1;

        public async Task<List<GetAllCurrenciesResponse>> GetCurrencies()
        {
            return await JsonSerializer.DeserializeAsync<List<GetAllCurrenciesResponse>>
             (await httpClient.GetStreamAsync($"api/v{apiversion}/Currency"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Currency> GetCurrencyById(int id)
        {
            return await JsonSerializer.DeserializeAsync<Currency>
                (await httpClient.GetStreamAsync($"api/v{apiversion}/Currency/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Currency> CreateCurrency(Currency newCurrency)
        {
            var currencyJson =
                new StringContent(JsonSerializer.Serialize(newCurrency), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"api/v{apiversion}/Currency", currencyJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Currency>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateCurrency(Currency updatedCurrency)
        {
            var currencyJson =
                 new StringContent(JsonSerializer.Serialize(updatedCurrency), Encoding.UTF8, "application/json");
            await httpClient.PutAsync($"api/v{apiversion}/Currency/{updatedCurrency.Id}", currencyJson);
            //public Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent content);
            //public        static       Task  PutJsonAsync(this HttpClient httpClient, string requestUri, object content);
        }

        public async Task DeleteCurrency(int id)
        {
            await httpClient.DeleteAsync($"api/v{apiversion}/Currency/{id}");
        }
    }
}
