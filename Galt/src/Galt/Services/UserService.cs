using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galt.Services
{
    public class UserService
    {
        // TODO : Actually use a gateway once it's a thing we have
        // readonly UserGateway _userGateway;

        // I'll use this till we get done with the above
        List<User> _users = new List<User>();

        readonly PasswordHasher _passwordHasher;

        public UserService( /*UserGateway userGateway,*/ PasswordHasher passwordHasher )
        {
            //_userGateway = userGateway;
            _passwordHasher = passwordHasher;
        }

        /*
        public bool CreatePasswordUser( string email, string password )
        {
            if ( _userGateway.FindByEmail( email ) != null ) return false;
            _userGateway.CreatePasswordUser( email, _passwordHasher.HashPassword( password ) );
            return true;
        }
        */

        public bool CreateOrUpdateGithubUser( string email, string accessToken )
        {
            // User user = _userGateway.FindByEmail( email );
            User user = _users.Find( u => u.Email == email );

            if ( user == null )
            {
                // _userGateway.CreateGithubUser( email, accessToken );
                _users.Add( new User { Email = email, GithubAccessToken = accessToken } );
                return true;
            }
            /*
            if ( user.GithubAccessToken == string.Empty )
            {
                _userGateway.AddGithubToken( user.UserId, accessToken );
            }
            else
            {
                _userGateway.UpdateGithubToken( user.UserId, accessToken );
            }
            */
            user.GithubAccessToken = accessToken;
            return false;
        }

        /*
        public User FindUser( string email, string password )
        {
            User user = _userGateway.FindByEmail( email );
            if ( user != null && _passwordHasher.VerifyHashedPassword( user.Password, password ) == PasswordVerificationResult.Success )
            {
                return user;
            }

            return null;
        }
        */

        public User FindUser( string email )
        {
            // return _userGateway.FindByEmail( email );
            return _users.First(u => u.Email == email);
        }

        public IEnumerable<string> GetAuthenticationProviders( string userId )
        {
            // return _userGateway.GetAuthenticationProviders( userId );
            return new List<string> { "GitHub"};
        }

        // TODO: Get this out once the DAL provides it
        // Bootleg User class to simulate user storage
        public class User
        {
            public int UserId { get; set; }

            public string Email { get; set; }

            public string GithubAccessToken { get; set; }
        }
    }
}
