using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NAdmin.Persistence
{
    public class NAdminDbContext : DbContext
    {
        public NAdminDbContext(DbContextOptions<NAdminDbContext> options) : base(options)
        {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        public static string DefaultSchema { get; } = "NAdmin";
        
        private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}