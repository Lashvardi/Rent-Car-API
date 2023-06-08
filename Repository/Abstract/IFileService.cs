namespace RentCar.Repository.Abstract;

public interface IFileService
{
    // This Interface Is Used For Saving Images To The Database.
    public Tuple<int, string> SaveImage(IFormFile imageFile);

    public bool DeleteImage(string imageFileName);
}
