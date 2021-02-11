using AspNetCoreHero.Results;
using Blazored.LocalStorage;
using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using StoreManager.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        protected readonly ILocalStorageService localStorage;
        protected readonly HttpClient httpClient;

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        private const int apiversion = 1;

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                TokenRequest authenticationRequest = new TokenRequest() { Email = email, Password = password };
                //var authenticationResponse = await httpClient.AuthenticateAsync(authenticationRequest);
                var RequestJson =
                new StringContent(JsonSerializer.Serialize(authenticationRequest), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync($"api/identity/token", RequestJson);

                if (response.IsSuccessStatusCode)
                {
                    var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var IOStream = await response.Content.ReadAsStreamAsync();                    
                    var authenticationResponse = await JsonSerializer.DeserializeAsync<Result<TokenResponse>>(IOStream, jsonSerializerOptions);
                    TokenResponse tokenResponse = authenticationResponse.Data;

                    if (tokenResponse.JWToken != string.Empty)
                    {
                        await localStorage.SetItemAsync("token", tokenResponse.JWToken);
                        ((CustomAuthenticationStateProvider)authenticationStateProvider).SetUserAuthenticated(email);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResponse.JWToken);
                        return true;
                    }
                } 
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Register(string firstName, string lastName, string userName, string email, string password)
        {
            RegisterRequest registrationRequest = new RegisterRequest() { FirstName = firstName, LastName = lastName, Email = email, UserName = userName, Password = password, ConfirmPassword = password };
            var RegisterJson =
           new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync($"api/identity/register", RegisterJson);

            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var IOStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<Result<string>>(IOStream, jsonSerializerOptions);
                if (result.Succeeded == true)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("token");
            ((CustomAuthenticationStateProvider)authenticationStateProvider).SetUserLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
