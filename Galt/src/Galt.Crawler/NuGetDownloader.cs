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

        public Dictionary<FrameworkName, IEnumerable<PackageDependency>> test( string packageId, SemanticVersion version )
        {
            var initialDic = GetDependenciesSpecificVersion(packageId, version);

            if( !initialDic.IsEmpty() )
            {
                foreach( IEnumerable<PackageDependency> listpackages in initialDic.Values )
                {
                    foreach( PackageDependency Initialpackage in listpackages )
                    {
                        var received = GetDependenciesSpecificVersion( Initialpackage.Id, Initialpackage.VersionSpec.MinVersion );
                        foreach( FrameworkName frameW in received.Keys )
                        {
                            if( initialDic.Keys.Any( e => e.FullName == frameW.FullName && e.Version.ToString() == frameW.Version.ToString() ) )
                            {
                                var frameWinInitial = initialDic.Keys.First(e => e.FullName == frameW.FullName && e.Version.ToString() == frameW.Version.ToString() );
                                foreach( PackageDependency package in received[frameW] )
                                {
                                    if(initialDic[frameWinInitial].Any(e => e.Id == package.Id && e.VersionSpec.MinVersion.Version.ToString() == package.VersionSpec.MinVersion.Version.ToString() ) )
                                    {

                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            else
                            {
                                initialDic.Add( frameW, received[frameW] );
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}