namespace AtlasBlog.Services.Interfaces;

public interface IImageService
{
    Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
    string ConvertByteArrayToFile(byte[] fileData, string extension);



}