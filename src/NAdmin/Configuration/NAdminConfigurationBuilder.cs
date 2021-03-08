using System;
using System.Reflection;

namespace NAdmin.Configuration
{
    public record NAdminConfiguration(string SqlServerConnectionString, Assembly[] EntitiesAssemblies, string MigrationAssembly);


    public class NAdminConfigurationBuilder
    {
        private NAdminConfiguration _configuration = new (string.Empty, null, string.Empty);

        public NAdminConfigurationBuilder UseSqlServerConnection(string connectionString)
            => new()
                {
                    _configuration = _configuration with { SqlServerConnectionString = connectionString }
                };

        public NAdminConfigurationBuilder SetEntitiesAssemblies(params Assembly[] assemblies) 
            => new()
            {
                _configuration = _configuration with { EntitiesAssemblies = assemblies }
            };

        public NAdminConfigurationBuilder SetMigrationAssembly(Assembly assembly)
            => new()
            {
                _configuration = _configuration with { MigrationAssembly = assembly.FullName }
            };
        
        public NAdminConfiguration Build()
        {
            if (_configuration.EntitiesAssemblies is null)
                throw new ArgumentNullException(nameof(_configuration.EntitiesAssemblies));
            
            return _configuration;
        }
    }
}