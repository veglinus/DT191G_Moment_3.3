namespace SongAPI;
using Microsoft.EntityFrameworkCore;

class SongsDB : DbContext
{
    public SongsDB(DbContextOptions options) : base(options) { }
    public DbSet<Song> Songs { get; set; }
}