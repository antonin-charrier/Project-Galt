using Microsoft.AspNetCore.Authentication.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Galt.Services.UserService;

namespace Galt.Authentication
{
    public interface IExternalAuthenticationManager
    {
        void CreateOrUpdateUser( OAuthCreatingTicketContext context );

        User FindUser( OAuthCreatingTicketContext context );
    }
}
