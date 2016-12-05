using Galt.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using static Galt.Services.UserService;

namespace Galt.Authentication
{
    public class GithubExternalAuthenticationManager : IExternalAuthenticationManager
    {
        readonly UserService _userService;

        public GithubExternalAuthenticationManager( UserService userService )
        {
            _userService = userService;
        }

        public void CreateOrUpdateUser( OAuthCreatingTicketContext context )
        {
            _userService.CreateOrUpdateGithubUser( context.GetEmail(), context.AccessToken );
        }

        public User FindUser( OAuthCreatingTicketContext context )
        {
            return _userService.FindUser( context.GetEmail() );
        }
    }
}
