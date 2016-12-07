using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static Galt.AzureManager.AzureManager;

namespace Galt.Authentication
{
    public class ExternalAuthenticationEvents
    {
        readonly IExternalAuthenticationManager _userManager;

        public ExternalAuthenticationEvents( IExternalAuthenticationManager userManager )
        {
            _userManager = userManager;
        }

        public Task OnCreatingTicket( OAuthCreatingTicketContext context )
        {
            _userManager.CreateOrUpdateUser( context );
            UserEntity user = _userManager.FindUser( context );
            ClaimsPrincipal principal = CreatePrincipal( user );
            context.Ticket = new AuthenticationTicket( principal, context.Ticket.Properties, CookieAuthentication.AuthenticationScheme );
            return Task.CompletedTask;
        }

        ClaimsPrincipal CreatePrincipal( UserEntity user )
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier, user.RowKey, ClaimValueTypes.String ),
                new Claim( ClaimTypes.Email, user.PartitionKey )
            };
            ClaimsPrincipal principal = new ClaimsPrincipal( new ClaimsIdentity( claims, "Cookies", ClaimTypes.Email, string.Empty ) );
            return principal;
        }
    }
}
