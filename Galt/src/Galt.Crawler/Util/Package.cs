using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galt.Crawler.Util
{
    public class Package
    {
        public Package(string packageId )
        {
            PackageId = packageId;
        }

        public string PackageId { get; }

        public List<VPackage> Vpackages { get; set; }

        public string Owner { get; set; }

        public string Description { get; set; }

        public void AddVpackage(VPackage vpackage)
        {
            if( vpackage != null ) throw new ArgumentException();
            Vpackages.Add( vpackage );
        }

        public bool RemoveVpackage(VPackage vpackage)
        {
            if( vpackage != null ) throw new ArgumentException();
            return Vpackages.Remove( vpackage );
        }

    }
}