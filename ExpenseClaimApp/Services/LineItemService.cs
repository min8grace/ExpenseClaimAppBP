using AutoMapper;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.LineItems.Commands.Update;
using StoreManager.Application.Features.LineItems.Queries.GetAllLineItems;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public class LineItemService : ILineItemService
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        public LineItemService(HttpClient httpClient, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.mapper = mapper;
        }
        private const int apiversion = 1;

        public async Task<List<GetAllLineItemsResponse>> GetLineItems()
        {
            return await JsonSerializer.DeserializeAsync<List<GetAllLineItemsResponse>>
             (await httpClient.GetStreamAsync($"api/v{apiversion}/LineItem"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<LineItem> GetLineItemById(int id)
        {
            return await JsonSerializer.DeserializeAsync<LineItem>
                (await httpClient.GetStreamAsync($"api/v{apiversion}/LineItem/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<LineItem> CreateLineItem(LineItem newLineItem)
        {
            var lineItemJson =
                new StringContent(JsonSerializer.Serialize(newLineItem), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"api/v{apiversion}/LineItem", lineItemJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<LineItem>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateLineItem(LineItem updatedLineItem)
        {
            var lineItemJson =
                 new StringContent(JsonSerializer.Serialize(updatedLineItem), Encoding.UTF8, "application/json");
            await httpClient.PutAsync($"api/v{apiversion}/LineItem/{updatedLineItem.Id}", lineItemJson);
            //public Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent content);
            //public        static       Task  PutJsonAsync(this HttpClient httpClient, string requestUri, object content);
        }

        public async Task DeleteLineItem(int id)
        {
            await httpClient.DeleteAsync($"api/v{apiversion}/LineItem/{id}");
        }
    }
}
