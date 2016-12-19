using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using NuGet;
using NUnit.Framework;

namespace Galt.Crawler.Tests
{
    [TestFixture]
    public class NuGetDownloadTests
    {
        [Test]
        public void Test_Package()
        {
            NuGetDownloader n = new NuGetDownloader();
            var p = n.FillPackage( "Code.Cake" );

            Assert.AreEqual( p.PackageId, "Code.Cake" );
            Assert.AreEqual( p.Vpackages.Last().Version.ToString(), "0.14.0.0" );
            Assert.AreEqual( p.Vpackages.Last().Dependencies.DicDependencies.First().Value.Last().PackageId, "Cake.Common");
        }

        [Test]
        public void Test_Serializer()
        {
            JsonSerializerPackage s = new JsonSerializerPackage();
            NuGetDownloader n = new NuGetDownloader();
            var p = n.FillPackage( "Code.Cake" );
            string result = s.JsonSerializePackage( p );

            Assert.NotNull( result );
        }
    }
}