using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Galt.Crawler.Util;
using NuGet;
using NUnit.Framework;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Galt.Crawler.Tests
{
    [TestFixture]
    public class NuGetDownloadTests
    {
        /*[Test]
        public void Test_Package()
        {
            NuGetDownloader n = new NuGetDownloader();
            var p = n.FillVPackage( "Code.Cake", n.GetLatestVersionPackage("Code.Cake"));

            Assert.AreEqual( p.PackageId, "Code.Cake" );
            //Assert.AreEqual( p.Vpackages.Last().Version.ToString(), "0.14.0.0" );
            //Assert.AreEqual( p.Vpackages.Last().Dependencies.DicDependencies.First().Value.Last().PackageId, "Cake.Common");
        }

        [Test]
        public void Test_Serializer()
        {
            JsonSerializerPackage s = new JsonSerializerPackage();
            NuGetDownloader n = new NuGetDownloader();
            VPackage vp = n.FillVPackage( "Code.Cake", n.GetLatestVersionPackage( "Code.Cake" ));
            n.GetDependenciesSpecificVersion(vp);
            string result = s.JsonSerializer( vp );

            // Set a variable to the My Documents path.
            string mydocpath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using( StreamWriter outputFile = new StreamWriter( mydocpath + @"\WriteLines.txt" ) )
            {
                outputFile.WriteLine( result );
            }

            Assert.NotNull( result );
        }*/

       [Test]
        public void Test_Serializer()
        {
            JsonSerializerPackage s = new JsonSerializerPackage();
            
            //StreamReader sr = new StreamReader("C:\\Users\\Léo\\Desktop\\WriteLines.txt");
            //string truc = sr.ReadLine();

            NuGetDownloader n = new NuGetDownloader();
            //VPackage vp = n.FillVPackage("Code.Cake", n.GetLatestVersionPackage("Code.Cake"));
            //n.GetDependenciesSpecificVersion(vp);
            GraphData gd = new GraphData();

            string json = File.ReadAllText(@"C:\Users\Léo\Desktop\WriteLines.txt");
            VPackage lol = JsonConvert.DeserializeObject<VPackage>(json);

            string truc = s.JsonSerializer(gd.ConvertGraphData(lol));

            // Set a variable to the My Documents path.
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\WriteLines.json"))
            {
                outputFile.WriteLine(truc);
            }
        }
    }
}