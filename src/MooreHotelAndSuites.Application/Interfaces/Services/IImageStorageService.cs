

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
   public interface IImageStorageService
{
     Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder);
    Task DeleteAsync(string imageUrl);
 
}
}
