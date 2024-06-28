namespace BandLabTestAssignment
{
    public static class Constants
    {
        public const string DatabaseConnectionString = "Database:ConnectionString";

        public const string StorageConnectionString = "Storage:ConnectionString";

        public const string StorageContainerName = "Storage:ContainerName";

        public const int ImageWidth = 600;

        public const int ImageHeight = 600;

        public const int CommentCount = 2;

        public static IReadOnlyList<string> AllowedExtensions { get; set; } = new List<string> { ".png", ".jpg", ".jpeg", ".bmp" };
    }
}
