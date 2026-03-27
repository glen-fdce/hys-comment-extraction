using GetComments.Entities;
using GetComments.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GetComments.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IOptions<DatabaseOptions> databaseOptions) : base(options)
    {
        _databaseOptions = databaseOptions;
    }
    
    public DbSet<EntityUser> Users { get; set; }
    public DbSet<EntityComment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityUser>()
            .HasMany(c => c.Comments)
            .WithOne();

        modelBuilder.Entity<EntityComment>()
            .HasOne(u => u.User)
            .WithMany(u => u.Comments);

        modelBuilder.Entity<EntityComment>()
            .HasOne<EntityComment>()
            .WithMany()
            .HasForeignKey(c => c.InReplyToId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}