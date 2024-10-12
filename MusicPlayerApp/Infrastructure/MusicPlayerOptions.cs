using System;

namespace MusicPlayerApp.Infrastructure;

public class MusicPlayerOptions
{
    public const string PlayerSettings="PlayerSettings";
    public int DefaultUserId { get; set; }
    public int DefaultPlaytime { get; set; }
    public int DefaultPageSize { get; set; }
    public int DefaultMaximumPlaylistSize { get; set; }

}
