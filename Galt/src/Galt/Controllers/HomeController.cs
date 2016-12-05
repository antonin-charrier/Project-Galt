using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Galt.Services;

namespace Galt.Controllers
{
    public class HomeController : Controller
    {
        readonly TokenService _tokenService;
        readonly UserService _userService;

        public HomeController( TokenService tokenService, UserService userService )
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ClaimsIdentity identity = User.Identities.FirstOrDefault( i => i.AuthenticationType == "Cookies" );
            if ( identity != null )
            {
                string userId = identity.FindFirst( ClaimTypes.NameIdentifier ).Value;
                string email = identity.FindFirst( ClaimTypes.Email ).Value;
                Token token = _tokenService.GenerateToken( userId, email );
                IEnumerable<string> providers = _userService.GetAuthenticationProviders( userId );
                ViewData[ "Token" ] = token;
                ViewData[ "Email" ] = email;
                ViewData[ "Providers" ] = providers;
            }
            else
            {
                ViewData[ "Token" ] = null;
                ViewData[ "Email" ] = null;
                ViewData[ "Providers" ] = null;
            }

            ViewData[ "NoLayout"] = true;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Galt allows you to manage all the dependencies from your projets and be aware of every issue or new version of a package.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Team Galt";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
