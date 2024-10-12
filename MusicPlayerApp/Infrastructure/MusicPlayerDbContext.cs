using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicPlayerApp.Models;

namespace MusicPlayerApp.Infrastructure;

public class MusicPlayerDbContext : DbContext
{
    private readonly IConfiguration configuration;

    public MusicPlayerDbContext(
        DbContextOptions<MusicPlayerDbContext> options,
        IConfiguration configuration)
        : base(options)
    {
this.configuration = configuration;
    }

    public DbSet<Genre> Genres { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Rating> Ratings{get;set;}
    public DbSet<UserPreference> UserPreferences { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite(
    //         configuration.GetConnectionString("MusicPlayerData"));
    //     base.OnConfiguring(optionsBuilder);
    // }

}
