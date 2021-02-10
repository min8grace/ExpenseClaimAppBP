using Blazored.LocalStorage;
using StoreManager.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Services.Base
{
    public class BaseDataService
    {
        protected readonly ILocalStorageService localStorage;

        protected readonly HttpClient httpClient; 

        public BaseDataService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;

        }

        //protected ApiResponse<Guid> ConvertApiExceptions<Guid>(ApiException ex)
        //{
        //    if (ex.StatusCode == 400)
        //    {
        //        return new ApiResponse<Guid>() { Message = "Validation errors have occured.", ValidationErrors = ex.Response, Success = false };
        //    }
        //    else if (ex.StatusCode == 404)
        //    {
        //        return new ApiResponse<Guid>() { Message = "The requested item could not be found.", Success = false };
        //    }
        //    else
        //    {
        //        return new ApiResponse<Guid>() { Message = "Something went wrong, please try again.", Success = false };
        //    }
        //}

        protected async Task AddBearerToken()
        {
            if (await localStorage.ContainKeyAsync("token"))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await localStorage.GetItemAsync<string>("token"));
        }
    }
}
