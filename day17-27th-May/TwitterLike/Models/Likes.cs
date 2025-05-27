using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TwitterLike.Models
{
    public class Likes
    {
        [Key]
        public int SerialNumber { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("TweetId")]

        public Tweet? tweet { get; set; }
        [ForeignKey("UserId")]
        public User? user { get; set; }
    }
}