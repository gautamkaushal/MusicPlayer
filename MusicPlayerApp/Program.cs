
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicPlayerApp.Infrastructure;
using MusicPlayerApp.Infrastructure.Player;

HostApplicationBuilder builder = 
    Host.CreateApplicationBuilder(args);
IConfiguration config = 
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

builder.Services
    .AddDbContext<MusicPlayerDbContext>(
        options =>
            options
                .UseSqlite(config.GetConnectionString("MusicPlayerData")));
builder.Services.AddTransient<Repository>();
builder.Services.AddTransient<PlayListManager>();
builder.Services.Configure<MusicPlayerOptions>(config.GetSection(MusicPlayerOptions.PlayerSettings));
builder.Services.AddSingleton<MusicPlayer>();

IHost host = builder.Build();
MusicPlayerOptions options = new();
config.GetSection(MusicPlayerOptions.PlayerSettings).Bind(options);

MusicPlayer musicPlayer = host.Services.GetRequiredService<MusicPlayer>();
musicPlayer.Play(
    options.DefaultUserId);

host.RunAsync();






