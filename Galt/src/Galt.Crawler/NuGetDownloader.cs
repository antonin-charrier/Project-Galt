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

        public async Task<VPackageEntity> GetInfoVPackage(string packageId, string version)
        {
            VPackageEntity vPEntity = new VPackageEntity(packageId, version);

            List<IPackage> packages = await Task.Factory.StartNew(() =>
            {
                return _repo.FindPackagesById(packageId).ToList();
            });
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

        public async Task<string> FillFullDependencies( VPackageEntity vp )
        {
            VPackage vP = await FillVPackage(vp.PartitionKey, vp.RowKey);
            return _jsonSeria.JsonSerializer( _graphData.ConvertGraphData( vP ) );
        }

        public async Task<PackageEntity> GetInfoPackage( string packageId )
        {
            PackageEntity pEntity = new PackageEntity(packageId);

            List<IPackage> packages = await Task.Factory.StartNew(() => {
                return _repo.FindPackagesById(packageId).ToList();
            });

            pEntity.Authors = packages.Last().Authors.ToList();
            pEntity.Description = packages.Last().Description;

            List<string> vpackages = new List<string>();
            packages.Reverse();
            foreach( IPackage item in packages )
            {
                vpackages.Add( item.Version.ToString() );
            }
            pEntity.ListVPackage = vpackages;

            return pEntity;
        }

        public async Task<string> GetLatestVersionPackage(string packageId)
        {
            List<IPackage> packages = await Task.Factory.StartNew(() =>
            {
                return _repo.FindPackagesById(packageId).ToList();
            });
            packages = packages.Where(item => (item.IsLatestVersion)).ToList();

            return packages.Last().Version.ToString();
        }

        public async Task<VPackage> FillVPackage( string packageId, string version )
        {
            List<IPackage> packages = await Task.Factory.StartNew(() => {
                return _repo.FindPackagesById(packageId).ToList();
            });
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
            await GetDependenciesSpecificVersion(vp);

            return vp;
        }

        public async Task GetDependenciesSpecificVersion(VPackage vPackage)
        {
            try
            {
                List<IPackage> packages = await Task.Factory.StartNew(() =>
                {
                    return _repo.FindPackagesById(vPackage.PackageId).ToList();
                });

                packages = packages.Where(p => (p.Version.Version.ToString() == vPackage.Version.ToString())).ToList();

                if (!packages.IsEmpty())
                {
                    vPackage.IsLastVersion = packages.First().IsLatestVersion;
                    Dictionary<string, IEnumerable<VPackage>> dicFrameDep = new Dictionary<string, IEnumerable<VPackage>>();
                    foreach (FrameworkName frameW in packages.First().GetSupportedFrameworks())
                    {
                        List<VPackage> listdep = new List<VPackage>();
                        foreach (var item in packages.First().GetCompatiblePackageDependencies(frameW))
                        {
                            VPackage vpackagedep = new VPackage(item.Id, item.VersionSpec.MinVersion.Version);
                            listdep.Add(vpackagedep);

                            Console.WriteLine(item.Id);
                            await GetDependenciesSpecificVersion(vpackagedep);
                        }
                        dicFrameDep.Add(frameW.FullName, listdep);
                    }
                    vPackage.Dependencies = dicFrameDep;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}