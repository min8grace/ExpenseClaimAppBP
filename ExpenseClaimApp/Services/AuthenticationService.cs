using Blazored.LocalStorage;
using ExpenseClaimApp.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services
{
    public class AuthenticationService : BaseDataService, IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider) : base(httpClient, localStorage)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                AuthenticationRequest authenticationRequest = new AuthenticationRequest() { Email = email, Password = password };
                var authenticationResponse = await _httpClient.AuthenticateAsync(authenticationRequest);

                if (authenticationResponse.Token != string.Empty)
                {
                    await _localStorage.SetItemAsync("token", authenticationResponse.Token);
                    ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserAuthenticated(email);
                    _httpClient.HttpHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticationResponse.Token);
                    return true;
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
            RegistrationRequest registrationRequest = new RegistrationRequest() { FirstName = firstName, LastName = lastName, Email = email, UserName = userName, Password = password };
            var response = await _httpClient.RegisterAsync(registrationRequest);

            if (!string.IsNullOrEmpty(response.UserId))
            {
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserLoggedOut();
            _httpClient.HttpHttpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
