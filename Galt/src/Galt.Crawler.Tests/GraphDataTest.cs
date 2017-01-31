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
    public class GraphDataTest
    {
        VPackage p1;

        [PreTest]
        public void Initialisation()
        {
            p1 = new VPackage("source", new Version());
            VPackage p2 = new VPackage("conflict1", new Version());
            VPackage p3 = new VPackage("conflict1", new Version());
            VPackage p4 = new VPackage("conflict2", new Version());
            VPackage p5 = new VPackage("conflict2", new Version());
            VPackage p6 = new VPackage("p6", new Version());

            p2.Dependencies = new Dictionary<string, IEnumerable<VPackage>>();
            List<VPackage> d2 = new List<VPackage>();
            d2.Add(p4);
            p1.Dependencies.Add("f2", d2);

            p3.Dependencies = new Dictionary<string, IEnumerable<VPackage>>();
            List<VPackage> d3 = new List<VPackage>();
            d3.Add(p5);
            d3.Add(p6);
            p1.Dependencies.Add("f3", d3);

            p1.Dependencies = new Dictionary<string, IEnumerable<VPackage>>();
            List<VPackage> d1 = new List<VPackage>();
            d3.Add(p2);
            d3.Add(p3);
            p1.Dependencies.Add("f1", d3);
        }

        [Test]
        public void Test_ConflictVersion()
        {
            p1 = new VPackage("source", new Version());
            VPackage p2 = new VPackage("conflict1", new Version());
            VPackage p3 = new VPackage("conflict1", new Version());
            VPackage p4 = new VPackage("conflict2", new Version());
            VPackage p5 = new VPackage("conflict2", new Version());
            VPackage p6 = new VPackage("p6", new Version());

            p2.Dependencies = new Dictionary<string, IEnumerable<VPackage>>();
            List<VPackage> d2 = new List<VPackage>();
            d2.Add(p4);
            p1.Dependencies.Add("f2", d2);

            p3.Dependencies = new Dictionary<string, IEnumerable<VPackage>>();
            List<VPackage> d3 = new List<VPackage>();
            d3.Add(p5);
            d3.Add(p6);
            p1.Dependencies.Add("f3", d3);

            p1.Dependencies = new Dictionary<string, IEnumerable<VPackage>>();
            List<VPackage> d1 = new List<VPackage>();
            d3.Add(p2);
            d3.Add(p3);
            p1.Dependencies.Add("f1", d3);

            GraphData gd = new GraphData();
            gd.ConvertGraphData(p1);
            Dictionary<string, List<Dictionary<string, string>>> graph = gd.Graph;

            Console.WriteLine(graph);
        }

        [Test]
        public void Test_NewVersion()
        {
            
        }
    }
}