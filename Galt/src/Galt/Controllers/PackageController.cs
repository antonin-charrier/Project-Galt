using System.Threading.Tasks;
using Galt.AzureManager;
using Galt.Crawler;
using Galt.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static Galt.AzureManager.Entities;

namespace Galt.Controllers
{
    [Route( "api/[controller]" )]
    public class PackageController
    {
        readonly PackageService _packageService;
        readonly JsonSerializerPackage JsonSeria;

        public PackageController(PackageService packageService)
        {
            _packageService = packageService;
            JsonSeria = new JsonSerializerPackage();
        }

        [HttpGet( "infoPackage" )]
        public async Task<string> GetPackageInfo(string packageId, string version = null)
        {
            PackageEntity p = await _packageService.GetPackage( packageId );
            string packageInfo = JsonSeria.JsonSerializer( p );

            VPackageEntity vP;
            if( version != null )
            {
                vP = await _packageService.GetVPackage( packageId, version );
            }
            else
            {
                vP = await _packageService.GetLastVPackage( packageId );
            }

            JObject rss = JObject.Parse(packageInfo);
            rss.Property( "ETag" ).Remove();
            rss.Property( "RowKey" ).Remove();
            rss.Property( "Timestamp" ).Remove();
            rss.Property( "PartitionKey" ).Remove();
            rss.Property( "Description" ).AddAfterSelf( new JProperty( "PublicationDate", vP.PublicationDate ) );
            rss.Property( "PublicationDate" ).AddAfterSelf( new JProperty( "Version", version ) );

            return rss.ToString();
        }

        [HttpGet( "graph" )]
        public string GetVpackageDependencies()
        {
            return "poopoopoopoo";
        }
    }
}