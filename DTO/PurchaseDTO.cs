using RentCar.models;

public class Purchase
{
    public int PurchaseID { get; set; }
    public string PhoneNumber { get; set; }
    public int CarId { get; set; }
    public int Multiplier { get; set; }

    // Navigation properties
    public User User { get; set; }
    public Car Car { get; set; }
}
