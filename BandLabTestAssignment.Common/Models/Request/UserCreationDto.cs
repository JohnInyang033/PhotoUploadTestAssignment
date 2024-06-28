namespace BandLabTestAssignment.Common.Models.Request
{
    public class UserCreationDto
    {
        public string UserName { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }
    }
}
