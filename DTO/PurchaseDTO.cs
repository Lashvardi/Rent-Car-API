using RentCar.models;

public class Purchase
{
    // Relationship For Purchasing Car
    // User Can Purchase Many Cars
    // Car Can Be Purchased By Many Users
    public int PurchaseID { get; set; }
    public string PhoneNumber { get; set; }
    public int CarId { get; set; }
    public int Multiplier { get; set; }

    // Navigation properties
    public User User { get; set; }
    public Car Car { get; set; }
}
