using Galt.AzureManager;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<bool> AddFavorite(string email, string packageId)
        {
            return await _aManager.UsersRequests.AddFavorite( email, packageId );
        }

        public async Task<bool> RemoveFavorite(string email, string packageId)
        {
            return await _aManager.UsersRequests.RemoveFavorite( email, packageId );
        }

        public string[] GetFavorites(string email)
        {
            UserEntity u = FindUser( email );
            JArray jArray = JArray.Parse(u.Favorites);
            string[] array = jArray.ToObject<string[]>();
            return array;
        }

        public UserEntity FindUser( string email )
        {
            return _aManager.UsersRequests.GetUser(email).Result;
        }

        public IEnumerable<string> GetAuthenticationProviders( string userId )
        {
            return new List<string> {"GitHub"};
        }
    }
}
