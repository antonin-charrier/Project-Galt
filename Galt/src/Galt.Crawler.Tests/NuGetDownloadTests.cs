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
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Galt.Crawler.Tests
{
    [TestFixture]
    public class NuGetDownloadTests
    {
        [Test]
        public void Test_Package()
        {
            NuGetDownloader n = new NuGetDownloader();
        }

        [Test]
        public void Test_FillVPackage()
        {
            NuGetDownloader n = new NuGetDownloader();
            string lastV = n.GetLatestVersionPackage("Newtonsoft.Json");
            var p = n.FillVPackage( "Newtonsoft.Json", lastV);

            Assert.AreEqual( p.PackageId, "Newtonsoft.Json" );
            Assert.IsFalse( p.Dependencies.IsEmpty() );
        }

        [Test]
        public void Test_JsonSerializer()
        {
            JsonSerializerPackage s = new JsonSerializerPackage();
            NuGetDownloader n = new NuGetDownloader();
            VPackage vp = n.FillVPackage( "Newtonsoft.Json", n.GetLatestVersionPackage( "Newtonsoft.Json" ));
            string result = s.JsonSerializer( vp );

            JObject rss = JObject.Parse( result );

            Console.WriteLine( rss );
            Assert.Pass();
        }

        //[Test]
        // public void Test_Serializer()
        // {
        //     JsonSerializerPackage s = new JsonSerializerPackage();

        //     //StreamReader sr = new StreamReader("C:\\Users\\Léo\\Desktop\\WriteLines.txt");
        //     //string truc = sr.ReadLine();

        //     NuGetDownloader n = new NuGetDownloader();
        //     //VPackage vp = n.FillVPackage("Code.Cake", n.GetLatestVersionPackage("Code.Cake"));
        //     //n.GetDependenciesSpecificVersion(vp);
        //     GraphData gd = new GraphData();

        //     string json = File.ReadAllText(@"C:\Users\Léo\Desktop\WriteLines.txt");
        //     VPackage lol = JsonConvert.DeserializeObject<VPackage>(json);

        //     string truc = s.JsonSerializer(gd.ConvertGraphData(lol));

        //     // Set a variable to the My Documents path.
        //     string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        //     // Write the string array to a new file named "WriteLines.txt".
        //     using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\WriteLines.json"))
        //     {
        //         outputFile.WriteLine(truc);
        //     }
        // }
    }
}