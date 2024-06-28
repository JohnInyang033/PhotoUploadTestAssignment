namespace BandLabTestAssignment.Common.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
