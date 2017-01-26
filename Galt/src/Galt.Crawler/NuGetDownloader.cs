using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Galt.Crawler.Util;
using NuGet;
using static Galt.AzureManager.Entities;
using System.Threading.Tasks;

namespace Galt.Crawler
{
    public class NuGetDownloader
    {
        IPackageRepository _repo;
        GraphData _graphData;
        JsonSerializerPackage _jsonSeria;
        private Task<List<IPackage>> vPackage;

        public NuGetDownloader()
        {
            _repo = PackageRepositoryFactory.Default.CreateRepository( "https://packages.nuget.org/api/v2" );
            _graphData = new GraphData();
            _jsonSeria = new JsonSerializerPackage();
        }

        public VPackageEntity GetInfoVPackage(string packageId, string version)
        {
            VPackageEntity vPEntity = new VPackageEntity(packageId, version);

            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();

            packages = packages.Where(item => (item.Version.ToString() == version)).ToList();

            string dateTime;

            // Savage fix for yet another of Thibaut's mistakes
            // This will prevent crashes but cause misbehavior
            // TODO: Find a better fix
            if (packages.Count > 0)
            {
                dateTime = packages.First().Published.ToString();
                dateTime = dateTime.Remove(10);
                string[] dateTimeSplit = dateTime.Split('/');
                if (dateTimeSplit.Length >= 2)
                {
                    string temp = dateTimeSplit[1];
                    dateTimeSplit[1] = dateTimeSplit[0];
                    dateTimeSplit[0] = temp;
                    dateTime = string.Join("/", dateTimeSplit);
                }
            }
            else dateTime = string.Empty;

            vPEntity.PublicationDate = dateTime;

            return vPEntity;
        }

        public string FillFullDependencies( VPackageEntity vp )
        {
            VPackage vP = FillVPackage(vp.PartitionKey, vp.RowKey);
            Dictionary<string, object> graphInfo = _graphData.ConvertGraphData( vP );

            if( !((List<Dictionary<string, string>>)graphInfo["versionConflict"]).IsEmpty() )
            {
                vP.Stat = "Alert";
            } else if( !((List<Dictionary<string, string>>)graphInfo["toUpdate"]).IsEmpty() )
            {
                vP.Stat = "Issue";
            } else
            {
                vP.Stat = "Ok";
            }

            return _jsonSeria.JsonSerializer( graphInfo );
        }

        public PackageEntity GetInfoPackage( string packageId )
        {
            PackageEntity pEntity = new PackageEntity(packageId);

            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();

            pEntity.Authors = (packages.Count > 0 ? packages.Last().Authors.ToList():new List<string>());
            pEntity.Description = (packages.Count > 0 ? packages.Last().Description : String.Empty);

            List<string> vpackages = new List<string>();
            packages.Reverse();
            foreach( IPackage item in packages )
            {
                vpackages.Add( item.Version.ToString() );
            }
            pEntity.ListVPackage = vpackages;

            return pEntity;
        }

        public string GetLatestVersionPackage(string packageId)
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where(item => (item.IsLatestVersion)).ToList();

            return packages.Last().Version.ToString();
        }

        public VPackage FillVPackage( string packageId, string version )
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();

            packages = packages.Where( item => (item.Version.ToString() == version) ).ToList();

            VPackage vp = new VPackage( packageId, packages.First().Version.Version );
            string dateTime = packages.First().Published.ToString();

            dateTime = dateTime.Remove( 10 );
            string[] dateTimeSplit = dateTime.Split('/');
            if ( dateTimeSplit.Length >= 2 )
            {
                string temp = dateTimeSplit[ 1 ];
                dateTimeSplit[ 1 ] = dateTimeSplit[ 0 ];
                dateTimeSplit[ 0 ] = temp;
                dateTime = string.Join( "/", dateTimeSplit );
            }

            vp.PublicationDate = dateTime;
            GetDependenciesSpecificVersion(vp);

            return vp;
        }

        public void GetDependenciesSpecificVersion(VPackage vPackage)
        {
            List<IPackage> packages = _repo.FindPackagesById(vPackage.PackageId).ToList();

            IEnumerable<IPackage> temp = packages.Where( p => p.IsLatestVersion );
            string lastV = !temp.IsEmpty() ? temp.First().Version.ToString() : String.Empty;
            packages = packages.Where(p => (p.Version.Version.ToString() == vPackage.Version.ToString())).ToList();

            if (!packages.IsEmpty())
            {
                vPackage.LastVersion = lastV;
                Dictionary<string, IEnumerable<VPackage>> dicFrameDep = new Dictionary<string, IEnumerable<VPackage>>();
                foreach (FrameworkName frameW in packages.First().GetSupportedFrameworks())
                {
                    List<VPackage> listdep = new List<VPackage>();
                    foreach (var item in packages.First().GetCompatiblePackageDependencies(frameW))
                    {
                        VPackage vpackagedep = new VPackage(item.Id, item.VersionSpec.MinVersion.Version);
                        listdep.Add(vpackagedep);

                        Console.WriteLine(item.Id);
                        GetDependenciesSpecificVersion(vpackagedep);
                    }
                    dicFrameDep.Add(frameW.FullName, listdep);
                }
                vPackage.Dependencies = dicFrameDep;
            }

        }
    }
}