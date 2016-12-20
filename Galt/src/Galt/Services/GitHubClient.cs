using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Galt.Services
{
    public class GitHubClient
    {
        public async Task<IEnumerable<string>> GetUserEmails( string githubAccessToken )
        {
            using ( HttpClient client = new HttpClient() )
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add( "Authorization", string.Format( "token {0}", githubAccessToken ) );
                headers.Add( "User-Agent", "Galt" );
                HttpResponseMessage response = await client.GetAsync( "https://api.github.com/user/emails" );

                using ( TextReader tr = new StreamReader( await response.Content.ReadAsStreamAsync() ) )
                using ( JsonTextReader jsonReader = new JsonTextReader( tr ) )
                {
                    JToken json = JToken.Load( jsonReader );
                    return json.Select( e => (string)e[ "email" ] ).ToList();
                }
            }
        }
    }
}
