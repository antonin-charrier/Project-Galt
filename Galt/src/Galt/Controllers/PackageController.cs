using System.Threading.Tasks;
using Galt.Crawler;
using Galt.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static Galt.AzureManager.Entities;
using Microsoft.AspNetCore.Authorization;
using Galt.Authentication;
using System.Security.Claims;

namespace Galt.Controllers
{
    [Route( "api/[controller]" )]
    public class PackageController : Controller
    {
        readonly PackageService _packageService;
        readonly UserService _userService;
        readonly JsonSerializerPackage JsonSeria;

        public PackageController( PackageService packageService, UserService userService )
        {
            _packageService = packageService;
            _userService = userService;
            JsonSeria = new JsonSerializerPackage();
        }

        [HttpGet( "infoPackage" )]
        public async Task<string> GetPackageInfo( string packageId, string version = null )
        {
            PackageEntity p = await _packageService.GetPackage( packageId );
            string packageInfo = JsonSeria.JsonSerializer( p );

            VPackageEntity vP;
            if ( version != null )
            {
                vP = await _packageService.GetVPackage( packageId, version );
            }
            else
            {
                vP = await _packageService.GetLastVPackage( packageId );
            }

            JObject rss = JObject.Parse( packageInfo );
            rss.Property( "ETag" ).Remove();
            rss.Property( "RowKey" ).Remove();
            rss.Property( "Timestamp" ).Remove();
            rss.Property( "PartitionKey" ).Remove();
            rss.Property( "Description" ).AddAfterSelf( new JProperty( "PublicationDate", vP.PublicationDate ) );
            rss.Property( "PublicationDate" ).AddAfterSelf( new JProperty( "Version", version ) );

            return rss.ToString();
        }

        [HttpGet( "LastVersion" )]
        public async Task<string> GetLastVersionpackage( string packageId )
        {
            VPackageEntity vP = await _packageService.GetLastVPackage( packageId );

            return vP.RowKey;
        }

        [HttpGet( "graph" )]
        public async Task<string> GetVpackageDependencies( string packageId, string version )
        {
            var returnValue = await _packageService.GetFullDependencies(packageId, version, true);
            return returnValue;
        }

        [HttpGet( "dependencies" )]
        public async Task<string> GetVpackageDependenciesForced( string packageId, string version )
        {
            return await _packageService.GetFullDependencies( packageId, version, true );
        }

        [HttpPost( "fav" )]
        [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
        public async Task<bool> Favorite( [FromBody]FavPayload payload )
        {
            if ( payload == null ) return false;
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            return await _userService.AddFavorite( email, payload.packageId );
        }

        [HttpPost( "unfav" )]
        [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
        public async Task<bool> Unfavorite( [FromBody]FavPayload payload )
        {
            if ( payload == null ) return false;
            string email = User.FindFirst( ClaimTypes.Email ).Value;
            return await _userService.RemoveFavorite( email, payload.packageId );
        }

        [HttpGet( "favorites" )]
        [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
        public async Task<string[]> ListFavorites()
        {
            return await Task.Run( () =>
            {
                string email = User.FindFirst( ClaimTypes.Email ).Value;
                return _userService.GetFavorites( email );
            } );
        }

        [HttpPost("isFav")]
        [Authorize( ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
        public async Task<bool> IsFavorite([FromBody] FavPayload payload)
        {
            return await Task.Run( () =>
             {
                 string email = User.FindFirst( ClaimTypes.Email ).Value;
                 string[] favorites = _userService.GetFavorites( email );

                 foreach ( string s in favorites )
                     if ( s == payload.packageId )
                         return true;

                 return false;
             } );
        }
    }

    public class FavPayload
    {
        public string packageId { get; set; }
    }
}