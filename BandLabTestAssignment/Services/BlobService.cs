using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BandLabTestAssignment.Extensions;
using BandLabTestAssignment.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BandLabTestAssignment.Services
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _config;

        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration config, BlobServiceClient blobServiceClient)
        {
            _config = config;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadBlobFileAsync(MemoryStream file, string fileName)
        {
            string uri = string.Empty;

            var containerClient = _blobServiceClient.GetBlobContainerClient(_config[Constants.StorageContainerName]);
            var blobClient = containerClient.GetBlobClient(fileName);

            using (var fileStream = file)
            {
                fileStream.Position = 0;
                var blockblob = await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = fileName.GetContentType() });
                return blobClient.Uri.ToString();
            }
        }
    }
}
