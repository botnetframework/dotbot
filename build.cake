var target = Argument<string>("target", "Default");
var config = Argument<string>("configuration", "Release");

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore("./src/Dotbot.sln", new DotNetCoreRestoreSettings
    {
        Verbose = false,
        Sources = new [] { "https://api.nuget.org/v3/index.json" }
    });
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Build
    DotNetCoreBuild("./src/Dotbot.sln", new DotNetCoreBuildSettings 
    {
        Configuration = config
    });
});

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);