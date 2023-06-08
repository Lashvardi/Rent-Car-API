using RentCar.models;

public class UserFavoriteCar
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; }
}
