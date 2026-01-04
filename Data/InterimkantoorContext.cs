
using Microsoft.Extensions.Options;

namespace Interimkantoor.Data
{
    public class InterimkantoorContext : IdentityDbContext<CustomUser>
    {
        public InterimkantoorContext(DbContextOptions<InterimkantoorContext> options) : base(options)
        {

        }

        public DbSet<Klant> Klanten { get; set; } = default!;
        public DbSet<Job> Jobs { get; set; } = default!;

        public DbSet<KlantJob> KlantJobs { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Klant>().ToTable("Klant");
            modelBuilder.Entity<Job>().ToTable("Job");
            modelBuilder.Entity<KlantJob>().ToTable("KlantJob");

            modelBuilder.Entity<KlantJob>()
                .HasOne(p => p.Klant)
                .WithMany(x => x.KlantJobs)
                .HasForeignKey(y => y.KlantId)
                .OnDelete(deleteBehavior:DeleteBehavior.Restrict);

            modelBuilder.Entity<KlantJob>()
                .HasOne(p => p.Job)
                .WithMany(x => x.KlantJobs)
                .HasForeignKey(y => y.JobId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        }
    }
}
