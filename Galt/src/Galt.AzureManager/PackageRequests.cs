using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using static Galt.AzureManager.Entities;
using System.Collections.Generic;

namespace Galt.AzureManager
{
    public class PackageRequests
    {
        public PackageRequests(AManager aManager)
        {
            AManager = aManager;
        }

        AManager AManager { get; }

        public async Task<bool> AddIfNotExists(string packageId, List<string> versions, string description, List<string> authors)
        {

            TableOperation retrieveOperation = TableOperation.Retrieve(packageId, "blbl");
            TableResult retrieved = await AManager.PackagesTable.ExecuteAsync(retrieveOperation);
            if( retrieved.Result != null ) return false;

            PackageEntity p = new PackageEntity(packageId);
            p.ListVPackage = versions;
            TableOperation insertOperation = TableOperation.Insert(p);
            await AManager.PackagesTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<PackageEntity> getPackage(string packageId)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve(packageId, "blbl");
            TableResult retrieved = await AManager.PackagesTable.ExecuteAsync(retrieveOperation);
            if( retrieved.Result != null ) return null;
            return (PackageEntity)retrieved.Result;
        }

        public async Task<bool> DeleteIfExists( string packageId )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<PackageEntity>(packageId, "blbl");
            TableResult retrieved = await AManager.PackagesTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            PackageEntity p = (PackageEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete(p);
            await AManager.PackagesTable.ExecuteAsync( removeOperation );
            return true;
        }
    }
}
