using AutoMapper;
using ExpenseClaimApp.Models;
using ExpenseClaimApp.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseClaimApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient<ICategoryService, CategoryService>
                        (client => client.BaseAddress = new Uri("https://localhost:44377/"));
            builder.Services.AddHttpClient<ICurrencyService, CurrencyService>
                        (client => client.BaseAddress = new Uri("https://localhost:44377/"));
            builder.Services.AddHttpClient<ILineItemService, LineItemService>
                       (client => client.BaseAddress = new Uri("https://localhost:44377/"));
            builder.Services.AddAutoMapper(typeof(CategoryProfile));
            builder.Services.AddAutoMapper(typeof(CurrencyProfile));
            builder.Services.AddAutoMapper(typeof(LineItemProfile));

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
