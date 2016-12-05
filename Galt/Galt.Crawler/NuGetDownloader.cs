using System;
using System.Collections.Generic;
using System.Linq;
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

        public ICollection<PackageDependency> GetDependenciesSpecificVersion( string packageId, SemanticVersion version )
        {
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where( p => (p.Version == version) ).ToList();

            if( !packages.IsEmpty() )
            {
                return packages.First().DependencySets.First().Dependencies;
            }
            return null;
        }

        public IEnumerable<PackageDependency> test( string packageId, SemanticVersion version )
        {
            List<PackageDependency> AllDependencies = new List<PackageDependency>();
            List<IPackage> packages = _repo.FindPackagesById(packageId).ToList();
            packages = packages.Where( p => (p.Version == version) ).ToList();

            bool added = true;
            if( !packages.IsEmpty() )
            {
                AllDependencies.AddRange( packages.First().DependencySets.First().Dependencies );
            }
            else
            {
                added = false;
            }

            while( added )
            {
                List<PackageDependency> tmpAllDep = AllDependencies;

                foreach( PackageDependency dep in AllDependencies )
                {
                    Console.WriteLine(GetDependenciesSpecificVersion( dep.Id, dep.VersionSpec.MaxVersion ).First());
                    if( !GetDependenciesSpecificVersion( dep.Id, dep.VersionSpec.MaxVersion ).IsEmpty() )
                    {
                        foreach( PackageDependency depOfDep in GetDependenciesSpecificVersion( dep.Id, dep.VersionSpec.MaxVersion ) )
                        {
                            if( !tmpAllDep.Contains( depOfDep ) )
                            {
                                tmpAllDep.Add( depOfDep );
                                added = true;
                            }
                        }
                    }
                    else
                    {
                        added = false;
                    }
                }
                AllDependencies.AddRange( tmpAllDep );
            };

            return AllDependencies;
        }
    }
}