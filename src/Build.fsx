#I @"packages/FAKE/tools"
#r "FakeLib.dll"

open Fake
open Fake.FileUtils
open Fake.TaskRunnerHelper

// Properties
let testOutput = FullName "TestResults"

let root = @".\"
let solutionName = "Hoverfly.Core.sln"
let solutionPath = root @@ solutionName
let outDir = getBuildParamOrDefault "outputDir" "./out/"

//--------------------------------------------------------------------
// Restore all packages

open Fake.RestorePackageHelper
Target "RestorePackages" (fun _ -> 
     solutionPath
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             OutputPath = root + "packages"
             Retries = 4 })
 )

//--------------------------------------------------------------------
// Clean build folder.

Target "Clean" (fun _ ->
  CleanDir outDir
)

//--------------------------------------------------------------------
// Build Release

Target "Build" <| fun _ ->
    !! solutionPath
    |> MSBuildRelease "" "Rebuild"
    |> ignore

Target "BuildRelease" DoNothing

//--------------------------------------------------------------------------------
// Copy the build output to bin directory

Target "CopyOutput" <| fun _ ->
    
    let copyOutput project =
        let src = root @@ project @@ @"bin/Release/"
        let dst = outDir @@ project
        CopyDir dst src allFiles
    [ "Hoverfly.Core" ]
    |> List.iter copyOutput

//--------------------------------------------------------------------------------
// Clean test output

Target "CleanTests" <| fun _ ->
    DeleteDir testOutput

//--------------------------------------------------------------------------------
// Run tests

open Fake.Testing
Target "RunTests" <| fun _ ->  
    let testAssemblies = !! (root @@ "**/bin/Release/Hoverfly.Core.Tests.dll")

    mkdir testOutput
    let xunitToolPath = findToolInSubPath "xunit.console.exe" (root @@ "packages/xunit.runner.console*/tools")

    printfn "Using XUnit runner: %s" xunitToolPath
    xUnit2
        (fun p -> { p with XmlOutputPath = Some (testOutput + @"\XUnitTestResults.xml"); HtmlOutputPath = Some (testOutput + @"\XUnitTestResults.HTML"); ToolPath = xunitToolPath; TimeOut = System.TimeSpan.FromMinutes 30.0; Parallel = ParallelMode.NoParallelization })
        testAssemblies

//--------------------------------------------------------------------
// Target Help

Target "Help" (fun _ ->
   trace "build [target]"
)

//--------------------------------------------------------------------
// Target dependencies

// build dependencies
"Clean" ==> "RestorePackages" ==> "Build" ==> "CopyOutput" ==> "BuildRelease"

// tests dependencies
"CleanTests" ==> "RunTests"

Target "All" DoNothing
"BuildRelease" ==> "All"
"RunTests" ==> "All"

// start build
RunTargetOrDefault "Help"