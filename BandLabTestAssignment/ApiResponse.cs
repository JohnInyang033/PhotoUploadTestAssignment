namespace BandLabTestAssignment
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; } 
        public List<string> Errors { get; set; } = new();

        public ApiResponse(bool success = true, T data = default)
        {
            Success = success; 
            Data = data;
        }
    }

    public class PostApiResponse<T> : ApiResponse<T>
    {
        public int Cursor { get; set; } 
    }
}
