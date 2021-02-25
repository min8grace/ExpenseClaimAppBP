using AutoMapper;
using Blazored.LocalStorage;
using ExpenseClaimApp.Auth;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpenseClaimApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddAuthorizationCore(); //for AuthorizationPolicyProvider
            builder.Services.AddScoped<CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>(
                provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());


            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("https://expenseclaimapi.azurewebsites.net")
                //BaseAddress = new Uri("https://localhost:44377")
            });

            builder.Services.AddHttpClient<ICategoryService, CategoryService>
                        (client => client.BaseAddress = new Uri("https://expenseclaimapi.azurewebsites.net"));
            builder.Services.AddHttpClient<ICurrencyService, CurrencyService>
                        (client => client.BaseAddress = new Uri("https://expenseclaimapi.azurewebsites.net"));
            builder.Services.AddHttpClient<ILineItemService, LineItemService>
                       (client => client.BaseAddress = new Uri("https://expenseclaimapi.azurewebsites.net"));
            builder.Services.AddHttpClient<IClaimService, ClaimService>
                       (client => client.BaseAddress = new Uri("https://expenseclaimapi.azurewebsites.net"));
            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>
                       (client => client.BaseAddress = new Uri("https://expenseclaimapi.azurewebsites.net"));

            builder.Services.AddAutoMapper(typeof(CategoryProfile));
            builder.Services.AddAutoMapper(typeof(CurrencyProfile));
            builder.Services.AddAutoMapper(typeof(LineItemProfile));
            builder.Services.AddAutoMapper(typeof(ClaimProfile));


            await builder.Build().RunAsync();
        }
    }
}
