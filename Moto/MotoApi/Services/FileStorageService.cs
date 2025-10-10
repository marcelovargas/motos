using MotoApi.Services.Interfaces;

namespace MotoApi.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

   
    public async Task<string> SaveImageFromBase64Async(byte[] imageBytes, string folder, string fileName, string mimeType)
    {
        
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (extension != ".png" && extension != ".bmp")
        {
            throw new ArgumentException("Only PNG and BMP files are allowed.");
        }

        
        var uploadsFolder = Path.Combine(_environment.WebRootPath, folder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        
        var filePath = Path.Combine(uploadsFolder, fileName);

        
        await File.WriteAllBytesAsync(filePath, imageBytes);

       
        return $"/{folder}/{fileName}";
    }

  
}