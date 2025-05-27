using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterLike.Models;

namespace TwitterLike.Contexts
{
    public class TwitterContext : DbContext
    {
        public TwitterContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<HashTag> HashTags { get; set; }
        public DbSet<TweetHashtag> Tweets_hashtags { get; set; }
        public DbSet<Follow> Follows { get; set; }






    }
}