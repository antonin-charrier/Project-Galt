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
            public UserEntity()
            {
                Favorites = "[]";
            }

            public UserEntity( string email )
            {
                PartitionKey = email;
                RowKey = email;
            }

            public string Favorites { get; set; }

            public string GitHubToken { get; set; }
        }

        public class VPackageEntity : TableEntity
        {
            public VPackageEntity() { }

            public VPackageEntity( string packageId, string version )
            {
                PartitionKey = packageId;
                RowKey = version;
            }
            public string PublicationDate { get; set; }

            public string FullDependencies { get; set; }

            public string StatOfDependencies { get; set; }
        }

        public class PackageEntity : TableEntity
        {
            public PackageEntity( string packageId )
            {
                PartitionKey = packageId;
                RowKey = "blbl";
            }

            public PackageEntity() { }

            public List<string> ListVPackage { get; set; }

            public string Description { get; set; }

            public List<string> Authors { get; set; }
        }
    }
}
