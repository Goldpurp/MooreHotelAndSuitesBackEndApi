using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using MooreHotelAndSuites.Application.Interfaces.Services;

public class CloudinaryImageStorageService : IImageStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageStorageService(IConfiguration config)
    {
        var account = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(account);
    }

 
   public async Task<string> UploadAsync(
    Stream fileStream,
    string fileName,
    string contentType,
    string folder)
{
    var uploadParams = new ImageUploadParams
    {
        File = new FileDescription(fileName, fileStream),
        Folder = folder,
        UseFilename = true,
        UniqueFilename = true,
        Overwrite = false
    };

    var result = await _cloudinary.UploadAsync(uploadParams);

    if (result.StatusCode != System.Net.HttpStatusCode.OK)
        throw new Exception("Image upload to Cloudinary failed");

    return result.SecureUrl.ToString();
}


    
    public async Task DeleteAsync(string imageUrl)
    {
        var publicId = ExtractPublicId(imageUrl);

        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);

        if (result.Result != "ok")
            throw new Exception("Failed to delete image from Cloudinary");
    }

    // PRIVATE HELPER
    private string ExtractPublicId(string url)
    {
        var uri = new Uri(url);
        var segments = uri.AbsolutePath.Split('/');

        var fileWithExt = segments.Last();                 // file.jpg
        var fileName = Path.GetFileNameWithoutExtension(fileWithExt); // file

        var folder = segments[^2];                         // rooms

        return $"{folder}/{fileName}";                     // rooms/file
    }
}
