using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using static Galt.AzureManager.Entities;

namespace Galt.AzureManager
{

    public class PackagesRequests
    {
        public PackagesRequests(AManager aManager)
        {
            AManager = aManager;
        }

        AManager AManager { get; }

        public async Task<bool> AddIfNotExists(string packageId, string version)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve(packageId, version);
            TableResult retrieved = await AManager.PackagesTable.ExecuteAsync(retrieveOperation);
            if( retrieved.Result != null ) return false;

            VPackageEntity p = new VPackageEntity(packageId, version);
            TableOperation insertOperation = TableOperation.Insert(p);
            await AManager.PackagesTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<VPackageEntity> getVPackage(string packageId, string version)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve(packageId, version);
            TableResult retrieved = await AManager.PackagesTable.ExecuteAsync(retrieveOperation);
            return (VPackageEntity)retrieved.Result;
        }

        public async Task<bool> AddDependenciesIfNotExist(string packageId, string version, string JsonSerializedPackage)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( packageId, version );
            TableResult retrieved = await AManager.PackagesTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            VPackageEntity p = (VPackageEntity)retrieved.Result;
            p.JsonPackage = JsonSerializedPackage;
            TableOperation modifyOperation = TableOperation.Replace(p);
            await AManager.PackagesTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> DeleteIfExists(string packageId, string version)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(packageId, version);
            TableResult retrieved = await AManager.PackagesTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            VPackageEntity p = (VPackageEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete(p);
            await AManager.PackagesTable.ExecuteAsync( removeOperation );
            return true;
        }
    }
}