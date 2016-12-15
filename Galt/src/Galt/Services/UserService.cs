using Galt.AzureManager;
using System.Collections.Generic;
using System.Linq;
using static Galt.AzureManager.AzureManager;

namespace Galt.Services
{
    public class UserService
    {
        // readonly UserGateway _userGateway;
        IAzureManager _azureManager;

        readonly PasswordHasher _passwordHasher;

        public UserService( PasswordHasher passwordHasher )
        {
            _azureManager = new AzureManager.AzureManager();
            _azureManager.Initialize();

            _passwordHasher = passwordHasher;
        }

        public bool CreateOrUpdateGithubUser( string email, string accessToken )
        {
            bool returnValue = _azureManager.AddIfNotExists( email ).Result;
            _azureManager.AddGitHubTokenIfExists( email, accessToken ).Wait();
            return returnValue;
        }

        public UserEntity FindUser( string email )
        {
            return _azureManager.GetUser(email).Result;
        }

        public IEnumerable<string> GetAuthenticationProviders( string userId )
        {
            return new List<string> { "GitHub"};
        }
    }
}
