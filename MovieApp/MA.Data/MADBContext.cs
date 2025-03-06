using MA.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MA.Data;

public class MADBContext : DbContext

{
    public MADBContext(DbContextOptions<MADBContext> options) :base(options)
    {
        
    }
    public DbSet<Movie> movies { get; set; }
    public DbSet<User> users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "User ID=myuser;Password=mypassword;Server=localhost;Port=5432;Database=mydatabase;Pooling=true;Maximum Pool Size=5;",
            options => options.MigrationsAssembly("MA.Data"));
    }
}