using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Galt.Crawler.Util;

namespace Galt.AzureManager
{
    public class PackagesRequests
    {
        public PackagesRequests(AManager aManager)
        {
            AManager = aManager;
        }

        AManager AManager { get; }

    }
}
