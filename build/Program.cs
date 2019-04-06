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
        private const string Build = "build";
        private const string Test = "test";
        private const string Pack = "pack";
        private const string Publish = "publish";

        public static Task Main(string[] args)
        {
            Target(Build, () => RunAsync("dotnet", "build PseudoLocalizer.sln -c Release"));

            Target(
                Test,
                DependsOn(Build),
                () => RunAsync("dotnet", $"test tests/PseudoLocalizer.Tests/PseudoLocalizer.Tests.csproj -c Release -r ../../{ArtifactsDir} --no-build -l trx;LogFileName=PseudoLocalizer.Tests.xml --verbosity=normal"));

            Target(
                Pack,
                DependsOn(Test),
                ForEach("PseudoLocalizer.nuspec", "PseudoLocalizer.Source.nuspec"),
                async nuspec =>
                {
                    Environment.SetEnvironmentVariable("NUSPEC_FILE", nuspec, EnvironmentVariableTarget.Process);
                    await RunAsync("dotnet", $"pack src/PseudoLocalizer/PseudoLocalizer.csproj -c Release -o ../../{ArtifactsDir} --no-build");
                });


            Target(Publish, DependsOn(Pack), () =>
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
                        Run("dotnet", $"nuget push {packageToPush} -k {apiKey} -s https://api.nuget.org/v3/index.json", noEcho: true);
                    }
                    catch (NonZeroExitCodeException) { } 
                }
            });

            Target("default", DependsOn(Pack, Publish));

            return RunTargetsAndExitAsync(args, ex => ex is NonZeroExitCodeException);
        }
    }
}
