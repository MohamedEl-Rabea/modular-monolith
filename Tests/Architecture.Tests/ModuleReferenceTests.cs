using System.Xml.Linq;
using FluentAssertions;

namespace Architecture.Tests;

public class ModuleReferenceTests
{
    [Fact]
    public void Modules_Should_Reference_Other_Modules_Only_Through_Contracts()
    {
        var root = FindRepoRoot();
        var modulesRoot = Path.Combine(root, "Modules");
        var serviceProjects = Directory.EnumerateFiles(modulesRoot, "*.Service.csproj", SearchOption.AllDirectories)
            .Where(p => !p.EndsWith(".Service.Tests.csproj", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var violations = new List<string>();

        foreach (var serviceProject in serviceProjects)
        {
            var serviceProjectDir = Path.GetDirectoryName(serviceProject)!;
            var currentModule = GetModuleNameFromPath(serviceProject);
            var references = ReadProjectReferences(serviceProject);

            foreach (var reference in references)
            {
                var referencedPath = Path.GetFullPath(Path.Combine(serviceProjectDir, reference));
                if (!referencedPath.Contains(Path.DirectorySeparatorChar + "Modules" + Path.DirectorySeparatorChar))
                {
                    continue;
                }

                var referencedModule = GetModuleNameFromPath(referencedPath);
                if (string.Equals(referencedModule, currentModule, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (IsContractsProject(referencedPath))
                {
                    continue;
                }

                violations.Add($"{Path.GetFileName(serviceProject)} -> {reference}");
            }
        }

        violations.Should().BeEmpty("modules must only reference other modules through contracts projects");
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "WT.B2C.API.sln")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Repository root not found.");
    }

    private static string GetModuleNameFromPath(string path)
    {
        var parts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        var modulesIndex = Array.FindIndex(parts, p => string.Equals(p, "Modules", StringComparison.OrdinalIgnoreCase));
        if (modulesIndex < 0 || modulesIndex + 1 >= parts.Length)
        {
            return string.Empty;
        }

        return parts[modulesIndex + 1];
    }

    private static bool IsContractsProject(string path)
    {
        return path.EndsWith("Contracts.csproj", StringComparison.OrdinalIgnoreCase) ||
               path.Contains(Path.DirectorySeparatorChar + "Contracts" + Path.DirectorySeparatorChar,
                   StringComparison.OrdinalIgnoreCase);
    }

    private static IEnumerable<string> ReadProjectReferences(string csprojPath)
    {
        var doc = XDocument.Load(csprojPath);
        return doc.Descendants("ProjectReference")
            .Select(e => e.Attribute("Include")?.Value)
            .Where(v => !string.IsNullOrWhiteSpace(v))!
            .Cast<string>();
    }
}
