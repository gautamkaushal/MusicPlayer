using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerApp.Models;

public class UserPreference
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int SongId { get; set; }
    public Song Song { get; set; }
    public int RatingId { get; set; }
    public Rating Rating { get; set; }
}
