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

        Task<bool> AddIfNotExists( string email, string pseudo );

        Task<UserEntity> GetUser( string email, string pseudo );

        Task<bool> AddGitHubTokenIfExists( string email, string pseudo, string token );

        Task<bool> DeleteIfExists( string email, string pseudo );
    }
}