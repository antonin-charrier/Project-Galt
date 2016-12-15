using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galt.Crawler.Util
{
    public class Dependencies
    {
        public Dependencies(VPackage vpackage)
            :this(vpackage, null)
        {
        }
        public Dependencies(VPackage vpackage, Dictionary<Framework, IEnumerable<VPackage>> dicDependencies)
        {
            Vpackage = vpackage;
            DicDependencies = dicDependencies;
        }

        public VPackage Vpackage { get; }

        public Dictionary<Framework, IEnumerable<VPackage>> DicDependencies { get; set; }

        public void AddDependency(Framework framework, IEnumerable<VPackage> Vpackages)
        {
            DicDependencies.Add( framework, Vpackages );
        }

        public bool RemoveDependency(Framework framework)
        {
            return DicDependencies.Remove( framework );
        }
    }
}