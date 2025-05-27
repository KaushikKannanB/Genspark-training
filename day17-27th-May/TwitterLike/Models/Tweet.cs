using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterLike.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TweetContent { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; }
        
        [ForeignKey("UserId")]
        public User? user { get; set; }

    }
}