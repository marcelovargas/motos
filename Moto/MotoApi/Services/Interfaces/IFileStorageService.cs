namespace MotoApi.Services.Interfaces;

public interface IFileStorageService
{
    
    Task<string> SaveImageFromBase64Async(byte[] imageBytes, string folder, string fileName, string mimeType);
   
}