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
            string temp = dateTimeSplit[1];
            dateTimeSplit[1] = dateTimeSplit[0];
            dateTimeSplit[0] = temp;
            dateTime = string.Join( "/", dateTimeSplit );

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
            vpackages.Reverse();
            foreach( IPackage item in packages )
            {
                vpackages.Add( item.Version.ToString() );
            }
            pEntity.ListVPackage = vpackages;

            return pEntity;
        }

        public IPackage GetLatestVersionPackage( string packageId )
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where( item => (item.IsLatestVersion) ).ToList();

            return packages.First();
        }

        public Package FillPackage( string packageId )
        {
            Package package = new Package(packageId);

            List<IPackage> packages = _repo.FindPackagesById(package.PackageId).ToList();
            List<VPackage> packagesProcessed = new List<VPackage>();
            foreach( var item in packages )
            {
                VPackage vp = new VPackage( package.PackageId, item.Version.Version );
                string dateTime = item.Published.ToString();

                dateTime = dateTime.Remove( 10 );
                string[] dateTimeSplit = dateTime.Split('/');
                string temp = dateTimeSplit[1];
                dateTimeSplit[1] = dateTimeSplit[0];
                dateTimeSplit[0] = temp;

                dateTime = string.Join( "/", dateTimeSplit );

                vp.PublicationDate = dateTime;
                packagesProcessed.Add( vp );
            }

            package.Authors = packages.Last().Authors.ToList();
            package.Vpackages = packagesProcessed;
            package.Description = packages.First().Description;
            foreach( var item in package.Vpackages )
            {
                item.Dependencies = new Dependencies( item );
                item.Dependencies.DicDependencies = GetDependenciesSpecificVersion( item );
            }
            return package;
        }

        public Dictionary<Framework, IEnumerable<VPackage>> GetDependenciesSpecificVersion( VPackage vPackage )
        {
            List<IPackage> packages = _repo.FindPackagesById(vPackage.PackageId).ToList();
            packages = packages.Where( p => (p.Version.Version.ToString() == vPackage.Version.ToString()) ).ToList();

            if( !packages.IsEmpty() )
            {
                Dictionary<Framework, IEnumerable<VPackage>> dicFrameDep = new Dictionary<Framework, IEnumerable<VPackage>>();
                foreach( FrameworkName frameW in packages.First().GetSupportedFrameworks() )
                {
                    List<VPackage> listdep = new List<VPackage>();
                    foreach( var item in packages.First().GetCompatiblePackageDependencies( frameW ) )
                    {
                        listdep.Add( new VPackage( item.Id, item.VersionSpec.MinVersion.Version ) );
                    }
                    dicFrameDep.Add( new Framework( frameW.FullName, frameW.Version ), listdep );
                }
                return dicFrameDep;
            }
            return null;
        }
    }
}