using Galt.Authentication;
using Galt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Galt.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class GitHubController : Controller
    {
        readonly GitHubService _gitHubService;

        public GitHubController( GitHubService gitHubService )
        {
            _gitHubService = gitHubService;
        }

        [HttpGet( "Emails" )]
        public async Task<ActionResult> GetUserEmails()
        {
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            IEnumerable<string> result = await _gitHubService.GetUserEmails( email );

            return result == null ? BadRequest() as ActionResult : Ok( result ) as ActionResult;
        }
    }
}
