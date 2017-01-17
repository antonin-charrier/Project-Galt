using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Galt.Crawler.Util;
using NuGet;
using static Galt.AzureManager.Entities;

namespace Galt.Crawler
{
    public class NuGetDownloader
    {
        IPackageRepository _repo;

        public NuGetDownloader()
        {
            _repo = PackageRepositoryFactory.Default.CreateRepository( "https://packages.nuget.org/api/v2" );
        }

        public VPackageEntity GetInfoVPackage(string packageId, string version )
        {
            VPackageEntity vPEntity = new VPackageEntity(packageId, version);

            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where( item => (item.Version.ToString() == version) ).ToList();

            string dateTime = packages.First().Published.ToString();
            dateTime = dateTime.Remove( 10 );
            string[] dateTimeSplit = dateTime.Split('/');
            if (dateTimeSplit.Length >= 2)
            {
                string temp = dateTimeSplit[ 1 ];
                dateTimeSplit[ 1 ] = dateTimeSplit[ 0 ];
                dateTimeSplit[ 0 ] = temp;
                dateTime = string.Join( "/", dateTimeSplit );
            }

            vPEntity.PublicationDate = dateTime;

            return vPEntity;
        }

        public PackageEntity GetInfoPackage( string packageId )
        {
            PackageEntity pEntity = new PackageEntity(packageId);

            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
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

        public string GetLatestVersionPackage( string packageId )
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where( item => (item.IsLatestVersion) ).ToList();

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
            string temp = dateTimeSplit[1];
            dateTimeSplit[1] = dateTimeSplit[0];
            dateTimeSplit[0] = temp;

            dateTime = string.Join( "/", dateTimeSplit );

            vp.PublicationDate = dateTime;

            return vp;
        }

        public void GetDependenciesSpecificVersion( VPackage vPackage )
        {
            List<IPackage> packages = _repo.FindPackagesById(vPackage.PackageId).ToList();
            packages = packages.Where( p => (p.Version.Version.ToString() == vPackage.Version.ToString()) ).ToList();

            if( !packages.IsEmpty() )
            {
                Dictionary<string, IEnumerable<VPackage>> dicFrameDep = new Dictionary<string, IEnumerable<VPackage>>();
                foreach( FrameworkName frameW in packages.First().GetSupportedFrameworks() )
                {
                    List<VPackage> listdep = new List<VPackage>();
                    foreach( var item in packages.First().GetCompatiblePackageDependencies( frameW ) )
                    {
                        VPackage vpackagedep = new VPackage( item.Id, item.VersionSpec.MinVersion.Version );
                        listdep.Add( vpackagedep );
                        GetDependenciesSpecificVersion( vpackagedep );
                    }
                    dicFrameDep.Add( frameW.FullName, listdep );
                }
                vPackage.Dependencies = dicFrameDep;
            }
        }
    }
}