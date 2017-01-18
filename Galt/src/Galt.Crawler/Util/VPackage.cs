using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galt.Crawler.Util
{
    public class VPackage
    { 
        public VPackage(string packageId, Version version)
        {
            if( packageId == null && version == null ) throw new ArgumentException();

            PackageId = packageId;
            Version = version;
            Dependencies = new Dictionary<string, IEnumerable<VPackage>>();
        }

        public string PackageId { get; }

        public Version Version { get; }

        public Dictionary<string, IEnumerable<VPackage>> Dependencies { get; set; }

        public string PublicationDate { get; set; }

    }
}