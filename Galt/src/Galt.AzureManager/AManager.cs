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

            VPackagesTable = CloudTableClient.GetTableReference( "VPackagesTable" );
            VPackagesTable.CreateIfNotExistsAsync();

            PackagesTable = CloudTableClient.GetTableReference( "PackagesTable" );
            PackagesTable.CreateIfNotExistsAsync();

            UsersRequests = new UsersRequests(this);
            VPackagesRequests = new VPackageRequests(this);
            PackagesRequests = new VPackageRequests(this);
        }

        public CloudTable UsersTable { get; }

        public CloudTable PackagesTable { get; }

        public CloudTable VPackagesTable { get; }

        public CloudStorageAccount CloudStorageAccount { get; }

        public CloudTableClient CloudTableClient { get; }

        public VPackageRequests VPackagesRequests { get; }

        public VPackageRequests PackagesRequests { get; }

        public UsersRequests UsersRequests { get; }
    }
}