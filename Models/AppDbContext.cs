using Microsoft.EntityFrameworkCore;

namespace thirdPractice.Models;
public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Music> Musics { get; set; }
    public DbSet<Game> Games { get; set; }
}