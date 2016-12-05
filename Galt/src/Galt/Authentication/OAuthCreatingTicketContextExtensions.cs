using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;

namespace Galt.Authentication
{
    public static class OAuthCreatingTicketContextExtensions
    {
        public static string GetEmail( this OAuthCreatingTicketContext @this )
        {
            return @this.Identity.FindFirst( c => c.Type == ClaimTypes.Email ).Value;
        }
    }
}
