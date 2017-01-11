using NuGet.Protocol.Core.Types;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol;
using System.Threading;
using NuGet.Common;
using Microsoft.Extensions.Logging;

namespace Galt.Services
{
    public class SearchService
    {
        public async Task<IEnumerable<IPackageSearchMetadata>> Search(string searchTerm)
        {
            List<Lazy<INuGetResourceProvider>> providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange( Repository.Provider.GetCoreV3() );
            PackageSource packageSource = new PackageSource( "https://api.nuget.org/v3/index.json" );
            SourceRepository sourceRepository = new SourceRepository( packageSource, providers );
            NuGet.Common.ILogger logger = new NullLogger();

            PackageSearchResource searchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>();
            IEnumerable<IPackageSearchMetadata> searchMetadata = await searchResource.SearchAsync( searchTerm, new SearchFilter(), 0, 10, logger, CancellationToken.None );
            return searchMetadata;
        }
    }
}
