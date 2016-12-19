using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Galt.AzureManager
{
    public class AManager
    {

        public AManager()
        {
            CloudStorageAccount = CloudStorageAccount.Parse( "UseDevelopmentStorage=true" );
            CloudTableClient = CloudStorageAccount.CreateCloudTableClient();

            UsersTable = CloudTableClient.GetTableReference( "UsersTable" );
            UsersTable.CreateIfNotExistsAsync();

            PackagesTable = CloudTableClient.GetTableReference( "PackagesTable" );
            PackagesTable.CreateIfNotExistsAsync();

            UsersRequests = new UsersRequests(this);
            PackagesRequests = new PackagesRequests(this);
        }

        public CloudTable UsersTable { get; }

        public CloudTable PackagesTable { get; }

        public CloudStorageAccount CloudStorageAccount { get; }

        public CloudTableClient CloudTableClient { get; }

        public PackagesRequests PackagesRequests { get; }

        public UsersRequests UsersRequests { get; }
    }
}