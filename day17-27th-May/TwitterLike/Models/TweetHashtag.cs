using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TwitterLike.Models
{
    public class TweetHashtag
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public int HashTagId { get; set; }

        [ForeignKey("TweetId")]
        public Tweet? tweet { get; set; }

        [ForeignKey("HashTagId")]
        public HashTag? hashtag { get; set; }
    }
}