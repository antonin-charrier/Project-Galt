using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galt.Crawler.Util
{
    public class Framework
    {
        public Framework(string frameworkId, Version version)
        {
            FrameworkId = frameworkId;
            Version = version;
        }

        public string FrameworkId { get; }

        public Version Version { get; }
    }
}
