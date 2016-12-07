using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galt.Crawler.Util
{
    public class VPackage
    {
        public VPackage(Package package, Version version)
            :this(package, package.PackageId, version)
        {
        }

        public VPackage(string packageId, Version version)
            :this(null, packageId, version)
        {
        }

        public VPackage(Package package, string packageId, Version version)
        {
            if( packageId != null && version != null ) throw new ArgumentException();

            Package = package;
            PackageId = packageId;
            Version = version;
        }

        public string PackageId { get; }

        public Version Version { get; }

        public Package Package { get; }

        public Dependencies Dependencies { get; set; }
    }
}
