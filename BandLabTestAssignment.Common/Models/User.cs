namespace BandLabTestAssignment.Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
