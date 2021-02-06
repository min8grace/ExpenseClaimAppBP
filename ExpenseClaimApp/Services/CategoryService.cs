using AutoMapper;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Categories.Commands.Update;
using StoreManager.Application.Features.Categories.Queries.GetAllCategories;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        public CategoryService(HttpClient httpClient, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.mapper = mapper;
        }
        private const int apiversion = 1;

        public async Task<List<GetAllCategoriesResponse>> GetCategories()
        {
            await AddBearerToken();
            return await JsonSerializer.DeserializeAsync<List<GetAllCategoriesResponse>>
             (await httpClient.GetStreamAsync($"api/v{apiversion}/Category"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await JsonSerializer.DeserializeAsync<Category>
                (await httpClient.GetStreamAsync($"api/v{apiversion}/Category/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Category> CreateCategory(Category newCategory)
        {
            var categoryJson =
                new StringContent(JsonSerializer.Serialize(newCategory), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"api/v{apiversion}/Category", categoryJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Category>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }    

        public async Task UpdateCategory(Category updatedCategory)
        {
            var categoryJson =
                 new StringContent(JsonSerializer.Serialize(updatedCategory), Encoding.UTF8, "application/json");
            await httpClient.PutAsync($"api/v{apiversion}/Category/{updatedCategory.Id}", categoryJson);
            //public Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent content);
            //public        static       Task  PutJsonAsync(this HttpClient httpClient, string requestUri, object content);
        }

        public async Task DeleteCategory(int id)
        {
            await httpClient.DeleteAsync($"api/v{apiversion}/Category/{id}");
        }
    }
}
