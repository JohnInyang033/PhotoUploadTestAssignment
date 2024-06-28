namespace BandLabTestAssignment.Services.Interfaces
{
    public interface IBlobService
    {
        public Task<string> UploadBlobFileAsync(MemoryStream file, string fileName);
    }
}
