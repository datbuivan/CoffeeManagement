using CloudinaryDotNet.Actions;

namespace CoffeeManagement.Interface
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams);
        Task<DeletionResult> DestroyAsync(DeletionParams deletionParams);
    }
}
