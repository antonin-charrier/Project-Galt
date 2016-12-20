using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Galt.AzureManager.AzureManager;

namespace Galt.Services
{
    public class GitHubService
    {
        UserService _userService;
        GitHubClient _gitHubClient;

        public GitHubService(UserService userService, GitHubClient gitHubClient)
        {
            _userService = userService;
            _gitHubClient = gitHubClient;
        }

        public async Task<IEnumerable<string>> GetUserEmails( string email )
        {
            UserEntity user = _userService.FindUser( email );
            if ( user == null ) return new string[0];
            if ( user.GitHubToken == string.Empty ) return new string[0];

            IEnumerable<string> logins = await _gitHubClient.GetUserEmails( user.GitHubToken );

            return logins;
        }
    }
}
