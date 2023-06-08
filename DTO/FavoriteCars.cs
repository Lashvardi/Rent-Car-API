using RentCar.models;

public class UserFavoriteCar
{
    // Relationship For Favoriting Car
    // User Can Favorite Many Cars
    // Car Can Be Favorited By Many Users
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; }
}
