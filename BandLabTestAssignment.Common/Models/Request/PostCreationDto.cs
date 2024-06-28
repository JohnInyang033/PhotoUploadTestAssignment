namespace BandLabTestAssignment.Common.Models.Request
{
    public class PostCreationDto
    {
        public string Caption { get; set; }
        public int Creator { get; set; } 

        public bool IsValid()
        {
            return Creator != default;
        }
    }
}
