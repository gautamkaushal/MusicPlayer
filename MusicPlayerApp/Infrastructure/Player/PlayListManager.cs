using System;
using Microsoft.Extensions.Options;
using MusicPlayerApp.Infrastructure;
using MusicPlayerApp.Models;

namespace MusicPlayerApp.Infrastructure.Player;

public class PlayListManager
{
    private readonly Repository repository;
    private readonly double phase1TimespanInMinutes;
    private readonly double phase2TimespanInMinutes;
    private readonly IOptions<MusicPlayerOptions> options;
    private readonly Random random;

    public PlayListManager(Repository repository, IOptions<MusicPlayerOptions> options)
    {
        this.options = options;

        this.repository = repository;
        this.random = new Random();
        this.phase1TimespanInMinutes =
            this.random.Next(15, 31) *
            this.random.NextDouble();
        this.phase2TimespanInMinutes =
            this.random.Next(30, 46) *
            this.random.NextDouble();
    }

    public User GetUser(int userId){
        return this.repository.GetUser(userId);
    }

    public ICollection<Genre> GetGenres(){
        return this.repository.GetGenres();
    }

    public ICollection<Song> GetRandomSongs(
        int userId,
        int genreId,
        TimeSpan sessionTimespan)
    {
        List<Song> songs = new();
        ICollection<int> songIds;

        songIds = 
            this.GetSongsManagerFactoryMethod(
                                userId, genreId, sessionTimespan)
                                .GetAllSongIds();
        int totalPages = songIds.Count / this.options.Value.DefaultPageSize;
        int randomPageNumber = this.random.Next(1, totalPages);
        List<int> selectedSongIds = songIds.Skip(randomPageNumber - 1).Take(this.options.Value.DefaultPageSize).ToList();
        for (int i = 0; i < this.options.Value.DefaultMaximumPlaylistSize; i++)
        {
            int randomIndex = this.random.Next(0, this.options.Value.DefaultPageSize);
            songs.Add(this.repository.GetSong(selectedSongIds[randomIndex], true));
        }

        return songs;
    }

    public void UpdateUserPreference(
            int userId,
            Song song,
            SongRating songRating)
    {
        this.repository
                .UpdateUserPreference(
                    userId,
                    song,
                    (int)songRating);
        this.repository.SaveChanges();
    }

     private SongsManager GetSongsManagerFactoryMethod(int userId, int genreId, TimeSpan sessionTimespan)
    {
        ICollection<UserPreference> preferences = this.repository.GetLikedPreferences(userId, genreId);
        if (preferences==null || preferences.Count <=0)
        {
            return new SongsManager(this.repository, userId, genreId);
        }
        SongsManager songsManager;
        if (sessionTimespan.TotalMinutes <= this.phase1TimespanInMinutes)
        {
            songsManager = new Phase1TimespanSongsManager(this.repository, userId, genreId);
        }
        else if (sessionTimespan.TotalMinutes <= this.phase2TimespanInMinutes)
        {
            songsManager = new Phase2TimespanSongsManager(this.random, this.repository, userId, genreId);
            
        }
        else
        {
            songsManager = new Phase3TimespanSongsManager(this.random, this.repository, userId, genreId);
        }

         if(preferences.Count <= this.options.Value.DefaultPageSize){
                songsManager.Additional = new SongsManager(this.repository, userId, genreId);
            }
        return songsManager;
    }
}
