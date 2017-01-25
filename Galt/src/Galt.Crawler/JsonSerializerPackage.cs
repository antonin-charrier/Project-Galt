using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galt.Crawler.Util;
using Newtonsoft.Json;

namespace Galt.Crawler
{
    public class JsonSerializerPackage
    {
        JsonSerializer _seria;

        public string JsonSerializer(object p)
        {
            _seria = new JsonSerializer();
            _seria.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            using( Stream s = new MemoryStream() )
            {
                using( StreamWriter sw = new StreamWriter( s, Encoding.UTF8, 2048, true ) )
                using( JsonWriter writer = new JsonTextWriter( sw ) )
                {
                    _seria.Serialize( writer, p );
                }

                using( StreamReader sr = new StreamReader( s, Encoding.UTF8 ) )
                {
                    s.Position = 0;
                    return sr.ReadToEnd();
                }
            }
        }

        public object JsonDeSeria(string s)
        {
            return JsonConvert.DeserializeObject( s );
        }
    }
}