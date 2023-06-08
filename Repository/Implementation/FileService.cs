using Microsoft.AspNetCore.Hosting;
using RentCar.Repository.Abstract;

namespace RentCar.Repository.Implementation;

public class FileService : IFileService
{

    private IWebHostEnvironment environment;

    public FileService(IWebHostEnvironment env)
    {
        this.environment = env;
    }


    public Tuple<int, string> SaveImage(IFormFile imageFile)
    {
        var contentPath = this.environment.ContentRootPath;
        var path = Path.Combine(contentPath, "Uploads");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var ext = Path.GetExtension(imageFile.FileName);
        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };

        if (!allowedExtensions.Contains(ext))
        {
            return new Tuple<int, string>(-1, "File extension is not allowed");
        }

        string uniqueString = Guid.NewGuid().ToString();

        var newFileName = uniqueString + ext;
        var fileWithPath = Path.Combine(path, newFileName);
        var stream = new FileStream(fileWithPath, FileMode.Create);
        imageFile.CopyTo(stream);
        stream.Close();

        var baseUrl = "https://localhost:7149/resources/";
        var fullImageUrl = baseUrl + newFileName;

        return new Tuple<int, string>(1, fullImageUrl);
    }

    public bool DeleteImage(string imageFileName)
    {
        try
        {
            var wwwPath = this.environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);

            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
        catch { return false; }
    }

}
