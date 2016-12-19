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
        public string JsonSerializePackage(Package p)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            Stream s = new MemoryStream();
            StreamWriter sw = new StreamWriter( s );
            JsonWriter writer = new JsonTextWriter( sw );
            StreamReader sr = new StreamReader( s, Encoding.UTF8 );

            serializer.Serialize( writer, p );
            s.Position = 0;
            return sr.ReadToEnd();
        }
    }
}