using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterLike.Models
{
    public class Follow
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FollowedById { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("FollowedById")]
        public User? Follower { get; set; }
        
    }
    
    
}