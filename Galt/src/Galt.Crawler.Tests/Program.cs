using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Common;
using NUnitLite;

namespace Galt.Crawler.Tests
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return new AutoRun( typeof( Program ).GetTypeInfo().Assembly )
                .Execute( args, new ExtendedTextWrapper( Console.Out ), Console.In );
        }
    }
}