using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using NuGet;

namespace Galt.Crawler
{
    public class NuGetDownloader
    {
        IPackageRepository _repo;

        public NuGetDownloader()
        {
            _repo = PackageRepositoryFactory.Default.CreateRepository( "https://packages.nuget.org/api/v2" );
        }

        public IPackage GetLatestVersionPackage( string packageId )
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where( item => (item.IsLatestVersion) ).ToList();

            return packages.First();
        }

        public IEnumerable<IPackage> GetAllVersionsPackage( string packageId )
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            return packages;
        }

        public Dictionary<FrameworkName, IEnumerable<PackageDependency>> GetDependenciesSpecificVersion( string packageId, SemanticVersion version )
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where( p => (p.Version == version) ).ToList();

            if( !packages.IsEmpty() )
            {
                Dictionary<FrameworkName, IEnumerable<PackageDependency>> dicFrameDep = new Dictionary<FrameworkName, IEnumerable<PackageDependency>>();
                foreach( FrameworkName frameW in packages.First().GetSupportedFrameworks() )
                {
                    dicFrameDep.Add( frameW, packages.First().GetCompatiblePackageDependencies( frameW ) );
                }
                return dicFrameDep;
            }
            return null;
        }
    }
}