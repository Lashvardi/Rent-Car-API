using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RentCar.models;
using System.Text.Json.Serialization;

// This Class Defines The User Model.
namespace RentCar.models
{
    public class User
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Role { get; set; }

        [JsonIgnore]
        public Car Car { get; set; }

        public List<UserFavoriteCar> FavoriteCars { get; set; }

        [JsonIgnore]
        public List<Purchase> Purchases { get; set; }

        public List<Message> Message { get; set; } = new List<Message>();
    }
}
