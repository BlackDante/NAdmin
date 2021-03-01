namespace NAdmin.Configuration
{
    public record NAdminConfiguration(string SqlServerConnectionString);


    public class NAdminConfigurationBuilder
    {
        private NAdminConfiguration _configuration = new (string.Empty);

        public NAdminConfigurationBuilder UseSqlServerConnection(string connectionString)
        {
            return new()
            {
                _configuration = _configuration with { SqlServerConnectionString = connectionString }
            };
        }
        
        public NAdminConfiguration Build()
        {
            return _configuration;
        }
    }
}