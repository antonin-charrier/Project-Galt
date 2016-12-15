using System.Collections.Generic;
using System.Linq;
using Galt.AzureManager;
using static Galt.AzureManager.Entities;

namespace Galt.Services
{
    public class UserService
    {
        // readonly UserGateway _userGateway;
        AManager _aManager;

        readonly PasswordHasher _passwordHasher;

        public UserService( PasswordHasher passwordHasher )
        {
            _aManager = new AManager();
            _passwordHasher = passwordHasher;
        }

        public bool CreateOrUpdateGithubUser( string email, string accessToken )
        {
            bool returnValue = _aManager.UsersRequests.AddIfNotExists( email ).Result;
            _aManager.UsersRequests.AddGitHubTokenIfExists( email, accessToken ).Wait();
            return returnValue;
        }

        public UserEntity FindUser( string email )
        {
            return _aManager.UsersRequests.GetUser(email).Result;
        }

        public IEnumerable<string> GetAuthenticationProviders( string userId )
        {
            return new List<string> { "GitHub"};
        }
    }
}
