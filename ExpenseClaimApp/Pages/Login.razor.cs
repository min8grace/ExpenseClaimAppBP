using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Pages
{
    public partial class Login
    {
        public LoginViewModel LoginViewModel { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public string Message { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        public Login()
        {

        }

        protected override void OnInitialized()
        {
            LoginViewModel = new LoginViewModel();
        }

        protected async void HandleValidSubmit()
        {
            if (await AuthenticationService.Authenticate(LoginViewModel.Email, LoginViewModel.Password))
            {
                await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("/");
            }
            Message = "Username/password combination unknown";
        }
    }
}
