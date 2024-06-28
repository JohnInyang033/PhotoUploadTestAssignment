namespace BandLabTestAssignment.Common.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; } 
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
