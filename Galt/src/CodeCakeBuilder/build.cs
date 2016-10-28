using System.Linq;
using Cake.Common.IO;
using Cake.Common.Solution;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.NuGet;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace CodeCake
{
    /// <summary>
    /// Sample build "script".
    /// It can be decorated with AddPath attributes that inject paths into the PATH environment variable. 
    /// </summary>
    [AddPath( "CodeCakeBuilder/Tools" )]
    public class Build : CodeCakeHost
    {
        public Build()
        {

            Task( "Clean" )
                .Does( () =>
                {
                    Cake.CleanDirectories( "**/bin/", d => !d.Path.Segments.Contains( "CodeCakeBuilder" ) );
                    Cake.CleanDirectories( "**/obj/", d => !d.Path.Segments.Contains( "CodeCakeBuilder" ) );
                } );

            Task( "Build" )
                .IsDependentOn( "Clean" )
                .Does( () =>
                {
                    using( var tempSln = Cake.CreateTemporarySolutionFile( "../../../../../../Galt.sln" ) )
                    {
                        tempSln.ExcludeProjectsFromBuild( "CodeCakeBuilder", "AnotherProjectIfNeeded" );
                        Cake.MSBuild( tempSln.FullPath, new MSBuildSettings()
                                .SetVerbosity( Verbosity.Minimal )
                                .SetMaxCpuCount( 1 ) );
                    }
                } );

            // The Default task for this script can be set here.
            Task( "Default" )
                .IsDependentOn( "Build" );
        }
    }
}