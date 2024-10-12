using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerApp.Models;

public class Song
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string Title { get; set; }
  public TimeSpan Time { get; set; }
  public int AlbumId { get; set; }
  public Album Album { get; set; }
  public int ArtistId { get; set; }
  public Artist Artist { get; set; }
  public int GenreId { get; set; }
  public Genre Genre { get; set; }
}
