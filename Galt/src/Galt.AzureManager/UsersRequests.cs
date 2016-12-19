using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using static Galt.AzureManager.Entities;

namespace Galt.AzureManager
{
    public class UsersRequests
    {

        public UsersRequests( AManager aManager)
        {
            AManager = aManager;
        }

        AManager AManager { get; }

        public async Task<bool> AddIfNotExists( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result != null ) return false;

            UserEntity u = new UserEntity( email );
            TableOperation insertOperation = TableOperation.Insert( u );
            await AManager.UsersTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<UserEntity> GetUser( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            return (UserEntity)retrieved.Result;
        }

        public async Task<bool> AddGitHubTokenIfExists( string email, string token )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            u.GitHubToken = token;
            TableOperation modifyOperation = TableOperation.Replace(u);
            await AManager.UsersTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> DeleteIfExists( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(email, email);
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete(u);
            await AManager.UsersTable.ExecuteAsync( removeOperation );
            return true;
        }
    }
}
