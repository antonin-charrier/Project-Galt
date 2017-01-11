using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using static Galt.AzureManager.Entities;

namespace Galt.AzureManager
{

    public class VPackageRequests
    {
        public VPackageRequests(AManager aManager)
        {
            AManager = aManager;
        }

        AManager AManager { get; }

        public async Task<bool> AddIfNotExists(string packageId, string version, string jsonSeria = null)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve(packageId, version);
            TableResult retrieved = await AManager.VPackagesTable.ExecuteAsync(retrieveOperation);
            if( retrieved.Result != null ) return false;

            VPackageEntity p = new VPackageEntity(packageId, version);
            p.JsonVPackage = jsonSeria;
            TableOperation insertOperation = TableOperation.Insert(p);
            await AManager.VPackagesTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<VPackageEntity> getVPackage(string packageId, string version)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve(packageId, version);
            TableResult retrieved = await AManager.VPackagesTable.ExecuteAsync(retrieveOperation);
            return (VPackageEntity)retrieved.Result;
        }

        public async Task<bool> AddDependenciesIfNotExist(string packageId, string version, string JsonSerializedVPackage)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( packageId, version );
            TableResult retrieved = await AManager.VPackagesTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            VPackageEntity p = (VPackageEntity)retrieved.Result;
            p.JsonVPackage = JsonSerializedVPackage;
            TableOperation modifyOperation = TableOperation.Replace(p);
            await AManager.VPackagesTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> DeleteIfExists(string packageId, string version)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<VPackageEntity>(packageId, version);
            TableResult retrieved = await AManager.VPackagesTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            VPackageEntity p = (VPackageEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete(p);
            await AManager.VPackagesTable.ExecuteAsync( removeOperation );
            return true;
        }
    }
}