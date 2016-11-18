using CodeCake;
using System;
using System.Linq;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace CodeCakeBuilder
{
    public static class Program
    {
        public static int Main( string[] args )
        {
            string solutionDir = Nth( Path.GetDirectoryName, PlatformServices.Default.Application.ApplicationBasePath, 6 );
            var app = new CodeCakeApplication( string.Format("{0}\\", solutionDir), typeof(Program).Assembly );
            bool interactive = !args.Contains( '-' + InteractiveAliases.NoInteractionArgument, StringComparer.OrdinalIgnoreCase );
            int result = app.Run( args );
            Console.WriteLine();
            if( interactive )
            {
                Console.WriteLine( "Hit any key to exit. (Use -{0} parameter to exit immediately)", InteractiveAliases.NoInteractionArgument );
                Console.ReadKey();
            }
            return result;
        }

        static T Nth<T>(Func<T, T> f, T input, int n)
        {
            for( int i = 0; i < n; i++ ) input = f( input );
            return input;
        }
    }
}