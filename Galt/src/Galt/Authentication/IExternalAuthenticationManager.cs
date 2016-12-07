using Microsoft.AspNetCore.Authentication.OAuth;
using static Galt.AzureManager.AzureManager;

namespace Galt.Authentication
{
    public interface IExternalAuthenticationManager
    {
        void CreateOrUpdateUser( OAuthCreatingTicketContext context );

        UserEntity FindUser( OAuthCreatingTicketContext context );
    }
}
