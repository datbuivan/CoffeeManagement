using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoffeeManagement.Interface;

namespace CoffeeManagement.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloud;

        public CloudinaryService(IConfiguration config)
        {
            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloud = new Cloudinary(account);
        }

        public Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams)
           => _cloud.UploadAsync(uploadParams);

        public Task<DeletionResult> DestroyAsync(DeletionParams deletionParams)
            => _cloud.DestroyAsync(deletionParams);

        
    }
}
