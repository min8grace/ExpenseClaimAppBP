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

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            //builder.Services.AddScoped<AuthenticationStateProvider, DummyAuthenticationStateProvider>();
            //builder.Services.AddAuthentication("Identity.Application").AddCookie();

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377")
            });

            builder.Services.AddHttpClient<ICategoryService, CategoryService>
                        (client => client.BaseAddress = new Uri("https://localhost:44377/"));
            builder.Services.AddHttpClient<ICurrencyService, CurrencyService>
                        (client => client.BaseAddress = new Uri("https://localhost:44377/"));
            builder.Services.AddHttpClient<ILineItemService, LineItemService>
                       (client => client.BaseAddress = new Uri("https://localhost:44377/"));
            builder.Services.AddHttpClient<IClaimService, ClaimService>
                       (client => client.BaseAddress = new Uri("https://localhost:44377/"));
            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>
                       (client => client.BaseAddress = new Uri("https://localhost:44377/"));

            builder.Services.AddAutoMapper(typeof(CategoryProfile));
            builder.Services.AddAutoMapper(typeof(CurrencyProfile));
            builder.Services.AddAutoMapper(typeof(LineItemProfile));
            builder.Services.AddAutoMapper(typeof(ClaimProfile));

            //services.AddHttpClient<IClaimService, ClaimService>(client =>
            //{
            //    client.BaseAddress = new Uri("https://localhost:44377/");
            //});

            //builder.Services.AddSingleton(new HttpClient
            //{
            //    BaseAddress = new Uri("https://localhost:5001")
            //});
            //builder.Services.AddHttpClient<IClient, Client>(client => client.BaseAddress = new Uri("https://localhost:5001"));
            //builder.Services.AddScoped<ICategoryService, CategoryService>();
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddHttpClient<ICountryDataService, CountryDataService>(client => client.BaseAddress = new Uri("https://localhost:44340/"));
            //builder.Services.AddHttpClient<IJobCategoryDataService, JobCategoryDataService>(client => client.BaseAddress = new Uri("https://localhost:44340/"));
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
