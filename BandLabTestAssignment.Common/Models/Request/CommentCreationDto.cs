namespace BandLabTestAssignment.Common.Models.Request
{
    public class CommentCreationDto
    { 
        public string Content { get; set; } 
        public int CreatorId { get; set; }
        public int PostId { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Content) && CreatorId != default && PostId != default;
        }
    }
}
