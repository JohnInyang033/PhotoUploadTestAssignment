using BandLabTestAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;

namespace BandLabTestAssignment.Services
{
    public class ImageService : IImageService
    {
        public async Task<MemoryStream> ResizeImage(IFormFile file, int width, int height)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using Image image = Image.FromStream(memoryStream);

            using Image resizedImage = new Bitmap(image, new Size(width, height));

            var outputMemoryStream = new MemoryStream();
            outputMemoryStream.Position = 0;
            resizedImage.Save(outputMemoryStream, GetImageFormat(file.FileName));

            return outputMemoryStream;
        }

        private ImageFormat GetImageFormat(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            switch (fileExtension)
            {
                case ".jpg":
                    return ImageFormat.Jpeg;

                case ".jpeg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                case ".bmp":
                    return ImageFormat.Bmp;

                default: return ImageFormat.Jpeg;
            }
        }
    }
}
