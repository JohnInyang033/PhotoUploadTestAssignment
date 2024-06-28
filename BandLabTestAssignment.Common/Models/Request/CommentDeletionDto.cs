namespace BandLabTestAssignment.Common.Models.Request
{
    public class CommentDeletionDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }

        public bool IsValid()
        {
            return Id != default && CreatorId != default;
        }
    }
}
