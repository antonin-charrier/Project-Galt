using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Galt.Services;
using NuGet.Protocol.Core.Types;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Galt.Controllers
{
    [Route( "api/[controller]" )]
    public class SearchController : Controller
    {
        readonly SearchService _searchService;

        public SearchController( SearchService searchService )
        {
            _searchService = searchService;
        }

        [HttpPost( "Search" )]
        public async Task<IActionResult> Search( [FromBody] SearchQuery query )
        {
            // Temporary line to look up the request.
            var request = Request;

            var results = await _searchService.Search( query.searchTerm );

            return Ok( results );
        }
    }

    public class SearchQuery
    {
        public string searchTerm { get; set; }
    }
}
