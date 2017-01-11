using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Galt.AzureManager;
using Galt.Crawler;
using static Galt.AzureManager.Entities;

namespace Galt.Services
{
    public class PackageService
    {
        VPackageRequests _vPackageReq;
        PackageRequests _packageReq;
        NuGetDownloader _nugetDL;

        public PackageService()
        {
            AManager manager = new AManager();
            _packageReq = new PackageRequests(manager);
            _vPackageReq = new VPackageRequests(manager);
            _nugetDL = new NuGetDownloader();
        }

        internal async Task<VPackageEntity> GetVPackage( string packageId, string version )
        {
            VPackageEntity VpackageEntity = await _vPackageReq.getVPackage(packageId, version);

            if( VpackageEntity == null )
            {
                VpackageEntity = new VPackageEntity( packageId, version );
                VpackageEntity.JsonVPackage = _nugetDL.GetInfoVPackage( packageId, version );

                await _vPackageReq.AddIfNotExists( packageId, version, VpackageEntity.JsonVPackage );
            }

            return VpackageEntity;
        }

        internal async Task<VPackageEntity> GetLastVPackage( string packageId )
        {
            PackageEntity pEntity = await GetPackage( packageId );
            string[] ArrayVersions = pEntity.ListVPackage.ToArray();
            string lastVersion = ArrayVersions[ArrayVersions.Length - 1];

            VPackageEntity vPEntity = await _vPackageReq.getVPackage(packageId, lastVersion);
            
            return vPEntity;
        }

        internal async Task<PackageEntity> GetPackage(string packageId)
        {
            PackageEntity packageEntity = await _packageReq.getPackage(packageId);

            if(packageEntity == null)
            {
                PackageEntity pEntity = _nugetDL.GetInfoPackage( packageId );
                await _packageReq.AddIfNotExists( pEntity.PartitionKey, pEntity.ListVPackage, pEntity.Description, pEntity.Authors );
                return pEntity;
            }

            return packageEntity;
        }
    }
}