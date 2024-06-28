using Microsoft.AspNetCore.Http;

namespace BandLabTestAssignment.Services.Interfaces
{
    public interface IImageService
    {
        Task<MemoryStream> ResizeImage(IFormFile file, int width, int height);
    }
}
