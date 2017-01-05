using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Galt.Services;
using NuGet.Protocol.Core.Types;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Galt.Controllers
{
    [Route( "api/[controller]" )]
    public class SearchController : Controller
    {
        readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search( [FromQuery] string searchTerm )
        {
            var results = await _searchService.Search( searchTerm );

            return Ok( results );
        }
    }
}
