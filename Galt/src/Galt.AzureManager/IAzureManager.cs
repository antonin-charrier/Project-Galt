using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Galt.AzureManager.AzureManager;

namespace Galt.AzureManager
{
    public interface IAzureManager
    {
        void Initialize();

        Task<bool> AddIfNotExists( string email );

        Task<UserEntity> GetUser( string email );

        Task<bool> AddGitHubTokenIfExists( string email, string token );

        Task<bool> DeleteIfExists( string email );
    }
}