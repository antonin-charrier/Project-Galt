using System;
using System.Collections.Generic;
using System.Linq;
using NuGet;
using NUnit.Framework;

namespace Galt.Crawler.Tests
{
    [TestFixture]
    public class NuGetDownloadTests
    {
        [Test]
        public void Test_GetLatestVersionPackage()
        {
            NuGetDownloader n = new NuGetDownloader();
            var package = n.GetLatestVersionPackage("Code.Cake");

            Assert.AreEqual( "Code.Cake", package.Id );
        }

        [Test]
        public void Test_GetAllVersionsPackage()
        {
            NuGetDownloader n = new NuGetDownloader();
            var packages = n.GetAllVersionsPackage("Code.Cake");

            foreach( IPackage p in packages )
            {
                Console.WriteLine( p.Version );
            }
        }

        [Test]
        public void Test_GetDependenciesSpecificVersion()
        {
            NuGetDownloader n = new NuGetDownloader();
            List<PackageDependency> dependencies = n.GetDependenciesSpecificVersion("Code.Cake", new SemanticVersion("0.14.0")).ToList();

            Assert.IsTrue( dependencies.Exists(i => i.Id == "Cake.Core" ) );
            Assert.IsTrue( dependencies.Exists(i => i.Id == "Cake.Common" ) );
        }

        [Test]
        public void test_test()
        {
            NuGetDownloader n = new NuGetDownloader();
            var result = n.test( "Code.Cake", new SemanticVersion( "0.14.0" ) );

            foreach( var i in result )
            {
                Console.WriteLine( i.Id );
            }
        }
    }
}
