using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Galt.AzureManager
{
    public class AzureManager : IAzureManager
    {
        CloudTable _usersTable;
        CloudStorageAccount _cloudStorageAccount;
        CloudTableClient _cloudTableClient;

        public void Initialize()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse( "UseDevelopmentStorage=true" );
            _cloudTableClient = _cloudStorageAccount.CreateCloudTableClient();
            _usersTable = _cloudTableClient.GetTableReference( "UsersTable" );
            _usersTable.CreateIfNotExistsAsync();
        }

        ////////////////////////////////////////////
        //Requests for Users
        ////////////////////////////////////////////

        public async Task<bool> AddIfNotExists( string email)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, null );
            TableResult retrieved = await _usersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result != null ) return false;

            UserEntity u = new UserEntity( email );
            TableOperation insertOperation = TableOperation.Insert( u );
            await _usersTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<UserEntity> GetUser(string email)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, null );
            TableResult retrieved = await _usersTable.ExecuteAsync( retrieveOperation );
            return (UserEntity)retrieved.Result;
        }

        public async Task<bool> AddGitHubTokenIfExists(string email, string token)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, null );
            TableResult retrieved = await _usersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            u.GitHubToken = token;
            TableOperation modifyOperation = TableOperation.Replace(u);
            await _usersTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> DeleteIfExists( string email)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(email, null);
            TableResult retrieved = await _usersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete(u);
            await _usersTable.ExecuteAsync( removeOperation );
            return true;
        }

        ////////////////////////////////////////////
        //Requests for Packages
        ////////////////////////////////////////////



        ////////////////////////////////////////////
        //Entities
        ////////////////////////////////////////////

        public class UserEntity : TableEntity
        {
            public UserEntity( string email)
            {
                PartitionKey = email;
                RowKey = null;
            }

            public UserEntity() { }

            public string Favorite { get; set; }

            public string GitHubToken { get; set; }
        }
    }
}
