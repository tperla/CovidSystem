using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
public interface IImageService
{
    Task<string> GetImageHash(IFormFile imageFile);
}
public class ImageService : IImageService
{
    // Method to compute hash of the image file
    public async Task<string> GetImageHash(IFormFile imageFile)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
        catch (Exception)
        {
            return "Error";
        }
    }
}

