using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.IO;
using Cake.Common.Tools.OpenCover;
using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.DotNetCore.Restore;
using System;

namespace CodeCake
{
    /// <summary>
    /// Sample build "script".
    /// It can be decorated with AddPath attributes that inject paths into the PATH environment variable. 
    /// </summary>
    public class Build : CodeCakeHost
    {
        public Build()
        {

            Task( "Clean" )
                .Does( () =>
                {
                    DirectoryPathCollection AllProj = Cake.GetDirectories( "./*", p => !p.Path.FullPath.Contains("CodeCakeBuilder" ));
                    foreach( DirectoryPath proj in AllProj )
                    {
                        if( Cake.DirectoryExists( proj + "/bin" ) )
                        {
                            Cake.DeleteDirectory( proj + "/bin", true );
                        }
                        if( Cake.DirectoryExists( proj + "/obj" ) )
                        {
                            Cake.DeleteDirectory( proj + "/obj", true );
                        }
                    }
                } );

            Task( "Restore" )
                .IsDependentOn( "Clean" )
                .Does( () =>
                {
                    Cake.DotNetCoreRestore();
                } );

            Task("Restore-Tools")
                .IsDependentOn("Restore")
                .Does( () =>
                {
                    DirectoryPath PackagesDir = new DirectoryPath("../packages");

                    DotNetCoreRestoreSettings dotNetCoreRestoreSettings = new DotNetCoreRestoreSettings();

                    dotNetCoreRestoreSettings.PackagesDirectory = PackagesDir;
                    dotNetCoreRestoreSettings.ArgumentCustomization = args => args.Append( "./CodeCakeBuilder/project.json" );

                    Cake.DotNetCoreRestore( dotNetCoreRestoreSettings );
                } );

            Task( "Build" )
                .IsDependentOn( "Restore-Tools" )
                .Does( () =>
                {
                    DirectoryPathCollection AllProj = Cake.GetDirectories( "./*", p => !p.Path.FullPath.Contains("CodeCakeBuilder" ));
                    foreach( DirectoryPath proj in AllProj )
                    {
                        Cake.DotNetCoreBuild( proj.FullPath );
                    }
                } );

            Task( "Unit-Tests" )
                .IsDependentOn( "Build" )
                .Does( () =>
                {
                    FilePathCollection FilePathTests = Cake.GetFiles("./**/*.Tests.exe", p => Cake.FileExists(p.Path+"/nunit.framework.dll"));

                    Cake.OpenCover( tool =>
                    {
                        tool.NUnit3( FilePathTests, new NUnit3Settings
                        {
                            ToolPath = "../packages/NUnit.ConsoleRunner/3.5.0/tools/nunit3-console.exe"
                        } );
                    },
                     new FilePath( "../resultOpenCover.xml" ),
                     new OpenCoverSettings
                     {
                         ToolPath = "../packages/OpenCover/4.6.519/tools/OpenCover.Console.exe",
                         Register = "User"
                     }
                        .WithFilter( "+[Test]*" )
                        .WithFilter( "-[Test.Tests]*" )
                    );
                } );

            // The Default task for this script can be set here.
            Task( "Default" )
                .IsDependentOn( "Unit-Tests" );
        }
    }
}