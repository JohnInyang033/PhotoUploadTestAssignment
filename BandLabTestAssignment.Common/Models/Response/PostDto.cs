namespace BandLabTestAssignment.Common.Models.Response
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; } 
        public DateTime CreatedAt { get; set; }
        public List<CommentDto> Comments { get; set; }

        public int CommentCount { get; set; }
    } 
}
