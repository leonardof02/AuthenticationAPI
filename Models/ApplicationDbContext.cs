using Microsoft.EntityFrameworkCore;

namespace MiddleApi.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    DbSet<User> Users { get; set; }
    DbSet<Article> Articles { get; set; }
    DbSet<UserData> UserDatas { get; set; }
    DbSet<Topic> Topics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("DataSource=app.db");
    }
}