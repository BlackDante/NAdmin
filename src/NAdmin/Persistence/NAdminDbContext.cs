using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NAdmin.Persistence
{
    public class NAdminDbContext : DbContext
    {
        private readonly IEnumerable<INAdminEntity> _entities;
        
        public NAdminDbContext(DbContextOptions<NAdminDbContext> options, IEnumerable<INAdminEntity> entities) : base(options)
        {
            _entities = entities;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityMethod = typeof(ModelBuilder).GetMethods()
                .Where(x => x.Name.Equals("Entity", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault(x => x.IsGenericMethod);
            var types = _entities.Select(e => e.GetType());

            foreach (var type in types)
            {
                entityMethod.MakeGenericMethod(type)
                    .Invoke(modelBuilder, new object[] {});
            }
            
            base.OnModelCreating(modelBuilder);
        }

        public static string DefaultSchema { get; } = "NAdmin";
        
        private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}