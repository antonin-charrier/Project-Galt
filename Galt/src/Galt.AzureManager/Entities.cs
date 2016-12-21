using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Galt.AzureManager
{
    public class Entities
    {
        public class UserEntity : TableEntity
        {
            public UserEntity( string email )
            {
                PartitionKey = email;
                RowKey = email;
            }

            public UserEntity() { }

            public string Favorite { get; set; }

            public string GitHubToken { get; set; }
        }

        public class VPackageEntity : TableEntity
        {
            public VPackageEntity( string packageId, string version )
            {
                PartitionKey = packageId;
                RowKey = version;
            }

            public string JsonPackage { get; set; }
        }
    }
}
