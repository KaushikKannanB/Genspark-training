using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TwitterLike.Models
{
    public class HashTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        
    }
}