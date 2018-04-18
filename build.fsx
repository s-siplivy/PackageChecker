// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = "./build/"
let solutionFile = "PackageChecker/PackageChecker.sln"
let solutionPackagesPath = "PackageChecker/packages"

// Targets
Target "Clean" (fun _ ->
  CleanDir buildDir
)

Target "RestorePackages" (fun _ -> 
  solutionFile
    |> RestoreMSSolutionPackages (fun p ->
      { p with
          OutputPath = solutionPackagesPath
          Retries = 4 })
 )

Target "BuildApp" (fun _ ->
  !! "PackageChecker/**/*.csproj"
    |> MSBuildRelease buildDir "Build"
    |> Log "AppBuild-Output: "
)

Target "Default" (fun _ ->
  trace "Build is complete!"
)

// Dependencies
"Clean"
  ==> "RestorePackages"
  ==> "BuildApp"
  ==> "Default"

// start build
RunTargetOrDefault "Default"
