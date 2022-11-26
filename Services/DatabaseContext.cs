using System.Reflection;
using Microsoft.EntityFrameworkCore;
using HumbleNote.Persistence.Models;

namespace HumbleNote.Services;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<HashTag> HashTags { get; set; }
    public DbSet<HashTagIndex> HashTagIndices { get; set; }
    public DbSet<NoteMention> NoteMentions { get; set; }


    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}