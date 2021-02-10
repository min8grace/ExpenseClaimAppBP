using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ExpenseClaimApp.Auth
{
    public class DummyAuthenticationStateProvider : AuthenticationStateProvider
    {

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            //var identity = new ClaimsIdentity("babo");
            var identity = new ClaimsIdentity(
                new List<Claim> {   new Claim(ClaimTypes.Name, "Dave") }
                , "babo"); // if changed to "", then it's not authorized.
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
         
        }
    }
}
