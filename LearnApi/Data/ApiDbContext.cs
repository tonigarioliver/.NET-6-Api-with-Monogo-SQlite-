using LearnApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace LearnApi.Data
{
    public class ApiDbContext:DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(p => p.Email)
                .IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(true);
            modelBuilder.Entity<RefreshToken>()
                   .HasOne(rt => rt.User)
                   .WithOne(u => u.RefreshToken)
                   .HasForeignKey<RefreshToken>(rt => rt.Tokenid)
                   .IsRequired(true);
            base.OnModelCreating(modelBuilder);
        }
    }
}
