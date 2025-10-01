// File: Data/AppDbContextFactory.cs
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MotoTrackAPI.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // procura appsettings subindo diretórios se necessário
            var basePath = Directory.GetCurrentDirectory();
            var appsettingsPath = FindAppsettings(basePath);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(appsettingsPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var connStr =
                Environment.GetEnvironmentVariable("ORACLE_CONNSTR")
                ?? configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException(
                    $"Connection string 'OracleConnection' não encontrada. Procurei em: {appsettingsPath}. " +
                    $"Defina ORACLE_CONNSTR ou inclua em appsettings*.json.");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseOracle(connStr, oracle =>
            {
                // 🔧 garante que as migrations deste projeto sejam usadas em design-time
                oracle.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);

                // Se quiser compat e sua versão suportar o enum, use:
                // oracle.UseOracleSQLCompatibility(OracleSQLCompatibility.Version12);
            });

            if (string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
            {
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
            }

            return new AppDbContext(optionsBuilder.Options);
        }

        /// <summary> Sobe diretórios até encontrar o appsettings.json. </summary>
        private static string FindAppsettings(string startPath)
        {
            var dir = new DirectoryInfo(startPath);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "appsettings.json");
                if (File.Exists(candidate)) return dir.FullName;
                dir = dir.Parent;
            }
            return startPath;
        }
    }
}
