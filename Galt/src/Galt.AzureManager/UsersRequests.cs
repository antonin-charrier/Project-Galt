using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using static Galt.AzureManager.Entities;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Galt.AzureManager
{
    public class UsersRequests
    {

        public UsersRequests( AManager aManager )
        {
            AManager = aManager;
        }

        AManager AManager { get; }

        public async Task<bool> AddIfNotExists( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if ( retrieved.Result != null ) return false;

            UserEntity u = new UserEntity( email );
            TableOperation insertOperation = TableOperation.Insert( u );
            await AManager.UsersTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<UserEntity> GetUser( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            return (UserEntity)retrieved.Result;
        }

        public async Task<bool> AddGitHubTokenIfExists( string email, string token )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if ( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            u.GitHubToken = token;
            TableOperation modifyOperation = TableOperation.Replace( u );
            await AManager.UsersTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> AddFavorite( string email, string packageId )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if ( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            if ( string.IsNullOrWhiteSpace(u.Favorites) ) u.Favorites = "[]";
            JArray array = JArray.Parse(u.Favorites);
            if ( array == null ) array = new JArray();
            array.Add( (JToken)packageId );
            u.Favorites = array.ToString();

            TableOperation modifyOperation = TableOperation.Replace( u );
            await AManager.UsersTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> RemoveFavorite( string email, string packageId )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if ( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            JArray array = JArray.Parse( u.Favorites );
            if ( string.IsNullOrWhiteSpace( u.Favorites ) ) u.Favorites = "[]";
            if ( array == null ) return true;
            while (array.Remove(packageId)); // Using a "while" to keep removing until none are left, in case of dupes
            u.Favorites = array.ToString();

            TableOperation modifyOperation = TableOperation.Replace( u );
            await AManager.UsersTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> DeleteIfExists( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>( email, email );
            TableResult retrieved = await AManager.UsersTable.ExecuteAsync( retrieveOperation );
            if ( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete( u );
            await AManager.UsersTable.ExecuteAsync( removeOperation );
            return true;
        }
    }
}
