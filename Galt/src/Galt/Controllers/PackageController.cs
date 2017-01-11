using System.Threading.Tasks;
using Galt.AzureManager;
using Galt.Crawler;
using Galt.Services;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<string> GetPackageInfo(string packageId)
        {
            PackageEntity p = await _packageService.GetPackage( packageId );
            string packageInfo = JsonSeria.JsonSerializer( p );
            return packageInfo;
        }

        [HttpGet("infoVPackage")]
        public async Task<string> GetVPackageInfo(string packageId, string version = null)
        {
            VPackageEntity vP;
            if(version != null)
            {
                vP = await _packageService.GetVPackage(packageId, version);
            } else
            {
                vP = await _packageService.GetLastVPackage(packageId);
            }

            return vP.JsonVPackage;
        }

        [HttpGet( "graph" )]
        public string GetVpackageDependencies()
        {
            return "poopoopoopoo";
        }
    }
}