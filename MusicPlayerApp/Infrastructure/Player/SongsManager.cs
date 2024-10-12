using System;
using MusicPlayerApp.Infrastructure;
using MusicPlayerApp.Models;

namespace MusicPlayerApp.Infrastructure;

public abstract class SongsManagerBase
{
    public SongsManager Additional { get; set; }

    public ICollection<int> GetAllSongIds()
    {
        ICollection<int> songIds, additionalSongIds;
        songIds = this.GetSongIds();
        if (this.Additional != null)
        {
            additionalSongIds = this.Additional.GetAllSongIds();
            return additionalSongIds.Union(songIds).ToList();
        }
        return songIds;
    }

    protected abstract ICollection<int> GetSongIds();
}

public class SongsManager:SongsManagerBase
{
    protected readonly Repository repository;
    protected readonly int userId;
    protected readonly int genreId;

    public SongsManager Additional { get; set; }

    public SongsManager(Repository repository, int userId, int genreId)
    {
        this.repository = repository;
        this.userId = userId;
        this.genreId = genreId;
    }

    public ICollection<int> GetAllSongIds()
    {
        ICollection<int> songIds, additionalSongIds;
        songIds = this.GetSongIds();
        if (this.Additional != null)
        {
            additionalSongIds = this.Additional.GetAllSongIds();
            return additionalSongIds.Union(songIds).ToList();
        }
        return songIds;
    }

    protected override ICollection<int> GetSongIds()
    {
        ICollection<int> songIds;
        songIds = this.repository.GetUnjudgedSongIds(this.userId, this.genreId);
        return songIds;
    }
}

public class Phase1TimespanSongsManager : SongsManager
{
    public Phase1TimespanSongsManager(
        Repository repository,
        int userId,
        int genreId)
    : base(repository, userId, genreId)
    {
    }
    protected override ICollection<int> GetSongIds()
    {
        return this.repository.GetLikedSongsIds(this.userId, this.genreId);
    }

}


public class Phase2TimespanSongsManager : SongsManager
{
    private readonly Random random;
    public Phase2TimespanSongsManager(
        Random random,
        Repository repository,
        int userId,
        int genreId)
    : base(repository, userId, genreId)
    {
        this.random = random;
    }
    protected override ICollection<int> GetSongIds()
    {
        double timePercentage =
           this.random.NextDouble() * this.random.Next(1, 11);
        if (timePercentage <= 9)
        {
            return this.repository.GetLikedSongsIds(userId, genreId);
        }
        else
        {
            return this.repository.GetUnjudgedSongIds(userId, genreId);
        }
    }

}


public class Phase3TimespanSongsManager : SongsManager
{
    private readonly Random random;
    public Phase3TimespanSongsManager(
        Random random,
        Repository repository,
        int userId,
        int genreId)
    : base(repository, userId, genreId)
    {
        this.random = random;
    }
    protected override ICollection<int> GetSongIds()
    {
        double timePercentage =
            this.random.NextDouble() * this.random.Next(1, 11);
        if (timePercentage <= 7.5)
        {
            return this.repository.GetLikedSongsIds(userId, genreId);
        }
        else if (timePercentage < 9.5)
        {
            return this.repository.GetUnjudgedSongIds(userId, genreId);
        }
        else
        {
            return this.repository.GetUnlikedSongsIds(userId, genreId);
        }
    }

}


