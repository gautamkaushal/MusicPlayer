using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Options;
using MusicPlayerApp.Infrastructure;
using MusicPlayerApp.Models;

namespace MusicPlayerApp.Infrastructure.Player;

public class MusicPlayer
{
    private readonly IOptions<MusicPlayerOptions> options;
    private readonly PlayListManager playlistManager;
    private readonly Stopwatch Timer;

    private const string Gap = "        ";

    public MusicPlayer(
        PlayListManager playlistManager,
        IOptions<MusicPlayerOptions> options)
    {
        this.options = options;
        this.playlistManager = playlistManager;
        this.Timer = new Stopwatch();
    }
    public void Play(int userId)
    {
        Console.Clear();
        Console.WriteLine("Choose options:");
        Console.WriteLine($"1. Start{Gap}2. Exit");
        int choice;
        int.TryParse(Console.ReadLine(), out choice);
        if (choice != 1)
            return;

        int selectedGenreId = GetGenreId();
        if (selectedGenreId ==0)
            return;
        
        User user = this.playlistManager.GetUser(userId);
        Console.Clear();
        Console.WriteLine($"...PLAYING for the user: {user.Name}...");
        this.Timer.Start();
        bool keepPlaying = true;
        while (keepPlaying)
        {
            ICollection<Song> songsToPlay =
                                    this.playlistManager
                                        .GetRandomSongs(userId,
                                                        selectedGenreId,
                                                        this.Timer.Elapsed);

            foreach (var song in songsToPlay)
            {
                Console.WriteLine();
                Console.WriteLine("Currently playing");
                Console.WriteLine($"Title: ***{song.Title}***");
                Console.WriteLine($@"Artist: {song.Artist.Name}{Gap}Genre: {song.Genre.Name}{Gap}TimeofPlay: {song.Time}");
                Console.WriteLine($"-------------You may choose to:{Gap}1. ThumbsUp{Gap}2. ThumbsDown{Gap}3. Next{Gap}4. Play till end{Gap}5. Exit");
                int songPreference = 0;
                if (!int.TryParse(Console.ReadLine(), out songPreference))
                {
                    continue;
                }
                switch (songPreference)
                {
                    case 1: //Playing till the end
                        Console.WriteLine("Playing till the end...");
                        Thread.Sleep(this.options.Value.DefaultPlaytime);
                        break;
                    case 2: //Thumbs Up
                            //Save preference
                        this.playlistManager.UpdateUserPreference(userId, song, SongRating.ThumbsUp);
                        Console.WriteLine("We saved your preference");
                        Console.WriteLine("Playing till the end...");
                        Thread.Sleep(this.options.Value.DefaultPlaytime);
                        break;
                    case 3: //Thumbs Down
                            //Save preference
                        this.playlistManager.UpdateUserPreference(userId, song, SongRating.ThumbsDown);
                        Console.WriteLine("We saved your preference");
                        Console.WriteLine("Moving to next...");
                        break;
                    case 4: //Play next song
                        continue;
                    default: //Exit
                             //Exit the app
                        keepPlaying = false;
                        break;
                }
                if(!keepPlaying) break;
            }
        }
    }

    private int GetGenreId()
    {
        ICollection<Genre> genres = this.playlistManager.GetGenres();
        StringBuilder stringBuilder = new StringBuilder("Choose genre: ");
        int i = 0;
        foreach (var genre in genres)
        {
            stringBuilder.Append($"{Gap}{++i}. {genre.Name}");
        }
        stringBuilder.Append($"{Gap}{++i}. Exit");
        Console.WriteLine(stringBuilder.ToString());
        int selectedGenreId;
        if (!int.TryParse(Console.ReadLine(), out selectedGenreId))
        {
            return 0;
        }
        if(selectedGenreId == i){
            return 0;
        }

        return selectedGenreId;
    }

}

