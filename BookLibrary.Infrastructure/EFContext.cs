using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.BookRecordAggregate;
using BookLibrary.Domain.Aggregates.ReservationAggregate;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BookLibrary.Infrastructure
{
    public class EFContext : DbContext, IUnitOfWork
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<BookRecord> BookRecord { get; set; }

        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
        }

        [ModuleInitializer]
        public static void Initialize()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _ = await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasIndex(p => p.Title)
                .IsUnique(true);

            modelBuilder.Entity<Book>()
                .Property(p => p.Status)
                .HasConversion<string>();
            
            modelBuilder.Entity<Book>()
                .Property(p => p.Genre)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(p => p.Email)
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .Property(p => p.Role)
                .HasConversion<string>();
            
            modelBuilder.Entity<BookRecord>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }

}
