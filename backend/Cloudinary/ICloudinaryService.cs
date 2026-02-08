using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace TodoListAPI.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(IFormFile? file);
    }
}
