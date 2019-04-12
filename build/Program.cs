using SimpleExec;
using System;
using System.IO;
using System.Threading.Tasks;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace Build
{
    internal class Program
    {
        private const string ArtifactsDir = "artifacts";
        private const string Clean = "clean";
        private const string Build = "build";
        private const string Test = "test";
        private const string Pack = "pack";
        private const string Publish = "publish";

        public static async Task Main(string[] args)
        {
            Target(Clean, () =>
            {
                var packagesToDelete = Directory.GetFiles(ArtifactsDir, "*.nupkg", SearchOption.TopDirectoryOnly);
                foreach (var package in packagesToDelete)
                {
                    File.Delete(package);
                }
            });

            Target(
                Build,
                DependsOn(Clean), 
                () => RunAsync("dotnet", "build PseudoLocalizer.sln -c Release"));

            Target(
                Test,
                DependsOn(Build),
                () => RunAsync("dotnet", $"test tests/PseudoLocalizer.Tests/PseudoLocalizer.Tests.csproj -c Release --no-build --verbosity=normal"));

            Target(
                Pack,
                DependsOn(Test),
                () => RunAsync("dotnet", $"pack src/PseudoLocalizer/PseudoLocalizer.csproj -c Release -o ../../{ArtifactsDir} --no-build"));

            Target(Publish, DependsOn(Pack), async () =>
            {
                var packagesToPush = Directory.GetFiles(ArtifactsDir, "*.nupkg", SearchOption.TopDirectoryOnly);
                Console.WriteLine($"Found packages to publish: {string.Join("; ", packagesToPush)}");

                var apiKey = Environment.GetEnvironmentVariable("NUGET_API_KEY");
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    Console.WriteLine("Nuget API key not available. Packages will not be pushed.");
                    return;
                }

                foreach (var packageToPush in packagesToPush)
                {
                    try
                    {
                        await RunAsync("dotnet", $"nuget push {packageToPush} -k {apiKey} -s https://api.nuget.org/v3/index.json", noEcho: true);
                    }
                    catch (NonZeroExitCodeException) { } 
                }
            });

            Target("default", DependsOn(Pack, Publish));

            await RunTargetsAndExitAsync(args, ex => ex is NonZeroExitCodeException);
        }
    }
}
