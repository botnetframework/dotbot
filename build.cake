#load "./scripts/version.cake"

var target = Argument<string>("target", "Default");
var config = Argument<string>("configuration", "Release");

var version = new BuildVersion("0.1.0", "local");
var projects = new DirectoryPath[] {
    "./src/Dotbot", "./src/Dotbot.Slack", "./src/Dotbot.Gitter"
};

var isRunningOnAppVeyor = BuildSystem.AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = BuildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;

Setup(context => 
{
    // Calculate semantic version.
    version = GetVersion(context);

    // Output some information.
    Information("Version: {0}", version.GetSemanticVersion());
    Information("Pull Request: {0}", isPullRequest);
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./.artifacts");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./src/Dotbot.sln", new DotNetCoreRestoreSettings
    {
        Verbose = false,
        Sources = new [] { "https://api.nuget.org/v3/index.json" }
    });
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Build
    DotNetCoreBuild("./src/Dotbot.sln", new DotNetCoreBuildSettings 
    {
        Configuration = config,
        ArgumentCustomization = args => args.Append("/p:Version={0}", version.GetSemanticVersion())
    });
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    // Publish
    foreach(var project in projects)
    {
        Information("\nPacking {0}...", project.FullPath);
        DotNetCorePack(project.FullPath, new DotNetCorePackSettings 
        {
            Configuration = config,
            OutputDirectory = "./.artifacts",
            VersionSuffix = version.Suffix,
            NoBuild = true,
            Verbose = false,
            ArgumentCustomization = args => args
                .Append("/p:Version={0}", version.GetSemanticVersion())
                .Append("--include-symbols --include-source")
        });
    }
});

Task("Publish")
    .WithCriteria(isRunningOnAppVeyor && !isPullRequest)
    .IsDependentOn("Pack")
    .Does(() =>
{
    var apiKey = EnvironmentVariable("DOTBOT_NUGET_API_KEY");
    if(string.IsNullOrWhiteSpace(apiKey))
    {
        throw new CakeException("NuGet API key was not provided.");
    }

    // Get the file paths.
    var packageVersion = version.GetSemanticVersion();
    var files = GetFiles("./.artifacts/*." + packageVersion + ".nupkg");

    foreach(var file in files) 
    {
        NuGetPush(file, new NuGetPushSettings() {
            Source = "https://nuget.org/api/v2/package",
            ApiKey = apiKey
        });
    }
});

Task("Default")
    .IsDependentOn("Pack");

Task("AppVeyor")
    .IsDependentOn("Publish");

RunTarget(target);