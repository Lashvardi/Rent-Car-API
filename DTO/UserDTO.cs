using Microsoft.EntityFrameworkCore;

namespace RentCar.DTO;

public class UserDTO
{
    // This Class Is Used For Inputting Data To The Database.
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }


    public string FirstName { get; set; }
    public string LastName { get; set; }


    public string Role { get; set; }
}