namespace AtlasBlog.Services.Interfaces;

public class BasicImageService : IImageService
{
    public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
    {
        try
        {
            using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream);
            byte[] byteFile = memoryStream.ToArray();
            return byteFile;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
    }

    public string ConvertByteArrayToFile(byte[] fileData, string extension)
    {
        try
        {
            var imageBase64Data = Convert.ToBase64String(fileData);
            
            return $"data:{extension};base64,{imageBase64Data}";
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}