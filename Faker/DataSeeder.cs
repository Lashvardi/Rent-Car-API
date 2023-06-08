using Bogus;
using RentCar.models;
using System.Collections.Generic;
using System.Text;
using RentCar.data;

// This Class Is Used To Generate Fake Data
// Implementation In Program.cs
namespace RentCar.Faker
{
    public static class DataSeeder
    {
        public static List<Car> SeedCars(IServiceProvider serviceProvider, int Quantity)
        {
            // Creating Faker For Car
            // Faker Is Used For Generating Fake Data
            var carFaker = new Faker<Car>()
                .RuleFor(c => c.Brand, f => f.Vehicle.Manufacturer())
                .RuleFor(c => c.Model, f => f.Vehicle.Model())
                .RuleFor(c => c.Year, f => f.Random.Number(1995, 2022))
                .RuleFor(c => c.ImageUrl1, f => $"https://loremflickr.com/320/240/car?random={f.UniqueIndex + 1}")
                .RuleFor(c => c.ImageUrl2, f => $"https://loremflickr.com/320/240/car?random={f.UniqueIndex + 3}")
                .RuleFor(c => c.ImageUrl3, f => $"https://loremflickr.com/320/240/car?random={f.UniqueIndex + 5}")
                .RuleFor(c => c.CreatedBy, f => f.Phone.PhoneNumberFormat())
                .RuleFor(c => c.Price, f => f.Random.Number(40, 95))
                .RuleFor(c => c.City, f => f.PickRandom("გორი", "თბილისი", "ქუთაისი", "ზესტაფონი", "ახალციხე", "ბათუმი", "ქობულეთი"))
                .RuleFor(c => c.Capacity, f => f.Random.Number(2, 8))
                .RuleFor(c => c.Transmission, f => f.PickRandom("ავტომატიკა", "მექანიკა"))
                .RuleFor(c => c.FuelCapacity, f => f.Random.Number(30, 100))
                .RuleFor(c => c.Latitude, f => f.Random.Double(41.60, 44.50))
                .RuleFor(c => c.Multiplier, f => f.PickRandom(1))
                .RuleFor(c => c.Longitude, f => f.Random.Double(43.30, 43.80));

            List<Car> cars = carFaker.Generate(Quantity);
            using (var scope = serviceProvider.CreateScope())
            {
                // Getting DbContext From Service Provider
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Cars.AddRange(cars);
                dbContext.SaveChanges();
            }

            return cars;
        }


        public static List<User> SeedUsers(IServiceProvider serviceProvider, int Quantity)
        {
            // Creating Faker For User
            var userFaker = new Faker<User>()
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.PasswordHash, f => Encoding.UTF8.GetBytes("1609"))
                .RuleFor(u => u.PasswordSalt, f => Encoding.UTF8.GetBytes("1609"))
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Role, f => f.PickRandom("User"));



            List<User> users = userFaker.Generate(Quantity);

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Users.AddRange(users);
                dbContext.SaveChanges();
            }

            return users;
        }
    }
}