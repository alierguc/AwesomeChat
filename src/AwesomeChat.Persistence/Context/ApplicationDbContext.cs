using AwesomeChat.Domain.Entities;
using AwesomeChat.Persistence.Common;
using AwesomeChat.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Persistence.Context
{
    public class ApplicationDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoomMapper());
            modelBuilder.ApplyConfiguration(new UserMapper());
            modelBuilder.ApplyConfiguration(new MessageMapper());
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-4KE4D8K;Initial Catalog=AwesomeChatDb; User Id=sa; Password=sa123456; Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;",
                builder => builder.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: new[] {4060}))
                .LogTo(filter: (eventId, level) => eventId == CoreEventId.ExecutionStrategyRetrying,
                logger:eventData =>
                {
                    Console.WriteLine($"SQL Bağlantı problemi. Tekrar bağlanmaya çalışılıyor.");
                });

            // Connection Resiliency
            */
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-4KE4D8K;Initial Catalog=AwesomeChatDb; User Id=sa; Password=sa123456; Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;",
               builder => builder.ExecutionStrategy(dependencies=>new MSSQLDatabaseExecutionStrategy(dependencies,3,TimeSpan.FromSeconds(15))));

        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

    
    }
}
