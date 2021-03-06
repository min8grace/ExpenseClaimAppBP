﻿using AutoMapper;
using Microsoft.AspNetCore.Components;
using StoreManager.Application.Features.Claims.Commands.Update;
using StoreManager.Application.Features.Claims.Queries.GetAllClaims;
using StoreManager.Application.Features.Claims.Queries.GetById;
using StoreManager.Application.Features.Claims.Queries.GetSearchClaims;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public class ClaimService : IClaimService
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        public ClaimService(HttpClient httpClient, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.mapper = mapper;
        }
        private const int apiversion = 1;

        public async Task<List<GetAllClaimsResponse>> GetClaims()
        {
            return await JsonSerializer.DeserializeAsync<List<GetAllClaimsResponse>>
             (await httpClient.GetStreamAsync($"api/v{apiversion}/Claim"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<GetAllClaimsResponse>> GetSearchClaims(string searchStr)
        {
            var IOStream = await httpClient.GetStreamAsync($"api/v{apiversion}/Claim/{searchStr}");
            var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var result = await JsonSerializer.DeserializeAsync<List<GetAllClaimsResponse>>(IOStream, jsonSerializerOptions);
            return result;
        }

        public async Task<Claim> GetClaimById(int id)
        {
            var IOStream = await httpClient.GetStreamAsync($"api/v{apiversion}/Claim/{id}");
            var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var result = await JsonSerializer.DeserializeAsync<GetClaimByIdResponse>(IOStream, jsonSerializerOptions);

            Claim claim = mapper.Map<Claim>(result);
            return claim;
            //return await JsonSerializer.DeserializeAsync<Claim>
            //    (await httpClient.GetStreamAsync($"api/v{apiversion}/Claim/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Claim> CreateClaim(Claim newClaim)
        {
            var claimJson =
                new StringContent(JsonSerializer.Serialize(newClaim), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync($"api/v{apiversion}/Claim", claimJson);

            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var IOStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<Claim>(IOStream, jsonSerializerOptions);
                return result;
            }
            return null;
        }

        public async Task UpdateClaim(Claim updatedClaim)
        {
            var claimJson =
                 new StringContent(JsonSerializer.Serialize(updatedClaim), Encoding.UTF8, "application/json");
            await httpClient.PutAsync($"api/v{apiversion}/Claim/{updatedClaim.Id}", claimJson);
            //public Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent content);
            //public        static       Task  PutJsonAsync(this HttpClient httpClient, string requestUri, object content);
        }

        public async Task DeleteClaim(int id)
        {
            await httpClient.DeleteAsync($"api/v{apiversion}/Claim/{id}");
        }
    }
}
