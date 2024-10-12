using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using MusicPlayerApp.Models;

namespace MusicPlayerApp.Infrastructure;

public class Repository
{
    private MusicPlayerDbContext context;
    public Repository(MusicPlayerDbContext context)
    {
        this.context = context;
    }

    public ICollection<Genre> GetGenres()
    {
        return this.context.Genres.ToList();
    }

    public User GetUser(int userId)
    {
        return this.context.Users
                         .Single(u => u.Id == userId);
    }

    public ICollection<UserPreference> GetLikedPreferences(
        int userId,
        int genreId)
    {
        var songs = this.context.Songs
                                .Where(s => s.GenreId == genreId);
        var userPreferences = this.context.UserPreferences
                                        .Where(u => u.UserId == userId
                                        && u.RatingId == (int)SongRating.ThumbsUp);
        return (from s in songs
                join p in userPreferences
                on s.Id equals p.SongId
                into preferredSongs
                from ps in preferredSongs
                select ps)
                .ToList();
    }

    public ICollection<int> GetUnjudgedSongIds(
        int userId,
        int genreId)
    {
        var judgedSongIds =
            this.context
                .UserPreferences
                .Include(p => p.Song)
                .Where(p => p.UserId == userId && p.Song.GenreId == genreId)
                .Select(p => p.SongId);
        var allSongIds =
            this.context
            .Songs
            .Where(s => s.GenreId == genreId)
            .Select(s => s.Id);

        return allSongIds
                .Except(judgedSongIds)
                .ToList();
    }

    public ICollection<int> GetLikedSongsIds(
        int userId,
        int genreId)
    {
        return this.context
                        .UserPreferences
                        .Include(p => p.Song)
                        .Where(
                            p => p.UserId == userId
                            && p.Song.GenreId == genreId
                            && p.RatingId == (int)SongRating.ThumbsUp)
                        .Select(p => p.SongId)
                        .ToList();
    }

    public ICollection<int> GetUnlikedSongsIds(
     int userId,
     int genreId)
    {
        return this.context
                        .UserPreferences
                        .Include(p => p.Song)
                        .Where(
                            p => p.UserId == userId
                            && p.Song.GenreId == genreId
                            && p.RatingId == (int)SongRating.ThumbsDown)
                        .Select(p => p.SongId)
                        .ToList();
    }

    public Song GetSong(int songId, bool includeAllAttributes = false)
    {
        if (includeAllAttributes)
        {
            return this.context.Songs
                .Include(s => s.Genre)
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Single(s => s.Id == songId);
        }
        return this.context.Songs
                        .Single(s => s.Id == songId);
    }

    public void UpdateUserPreference(
        int userId,
        Song song,
        int rating)
    {
        this.context.UserPreferences
            .Add(new UserPreference
            {
                UserId = userId,
                SongId = song.Id,
                RatingId = rating
            });
    }

    public void SaveChanges()
    {
        this.context.SaveChanges();
    }


}

