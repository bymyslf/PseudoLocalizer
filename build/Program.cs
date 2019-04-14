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
            Target(Clean, CleanDirectory);

            Target(
                Build,
                DependsOn(Clean),
                () => RunAsync("dotnet", "build PseudoLocalizer.sln -c Release"));

            Target(
                Test,
                DependsOn(Build),
                () => RunAsync("dotnet", $"test tests/PseudoLocalizer.Tests/PseudoLocalizer.Tests.csproj -c Release --no-build --verbosity=normal"));

            if (IsTravis() && IsMasterBranch())
            {
                Target(
                    Pack,
                    DependsOn(Test),
                    () => RunAsync("dotnet", $"pack src/PseudoLocalizer/PseudoLocalizer.csproj -c Release -o ../../{ArtifactsDir} --no-build"));

                Target(Publish, DependsOn(Pack), PublishPackages);

                Target("default", DependsOn(Pack, Publish));
            }
            else
            {
                Target("default", DependsOn(Test, Build));
            }
            
            await RunTargetsAndExitAsync(args, ex => ex is NonZeroExitCodeException);

            bool IsTravis()
                => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TRAVIS_OS_NAME"));

            bool IsMasterBranch()
            {
                var travisBranch = Environment.GetEnvironmentVariable("TRAVIS_BRANCH");
                if (!string.IsNullOrWhiteSpace(travisBranch) && travisBranch.Equals("master"))
                {
                    return true;
                }

                return false;
            }

            void CleanDirectory()
            {
                var packagesToDelete = Directory.GetFiles(ArtifactsDir, "*.nupkg", SearchOption.TopDirectoryOnly);
                foreach (var package in packagesToDelete)
                {
                    File.Delete(package);
                }
            }

            async Task PublishPackages()
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
            }
        }
    }
}