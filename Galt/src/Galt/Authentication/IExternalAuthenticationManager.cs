using Microsoft.AspNetCore.Authentication.OAuth;
using static Galt.AzureManager.Entities;

namespace Galt.Authentication
{
    public interface IExternalAuthenticationManager
    {
        void CreateOrUpdateUser( OAuthCreatingTicketContext context );

        UserEntity FindUser( OAuthCreatingTicketContext context );
    }
}
