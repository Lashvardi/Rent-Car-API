using Microsoft.AspNetCore.Mvc;
using RentCar.models;
using RentCar.DTO;
using RentCar.data;
using Microsoft.EntityFrameworkCore;
using RentCar.SMTP;
using System.Globalization;

namespace RentCar.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PurchaseController : Controller
    {

        private readonly DataContext _context;
        private readonly GetRentMail _GetRentEmail;
        private readonly SendRentMail _sendMail;

        public PurchaseController(DataContext context, GetRentMail getRentEmail, SendRentMail sendMail)
        {
            _context = context;
            _GetRentEmail = getRentEmail;
            _sendMail = sendMail;
        }

        // Sending Post Request To Purchase Car
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseCar(string phoneNumber, int carId, int multiplier = 1)
        {
            var user = _context.Users.SingleOrDefault(u => u.PhoneNumber == phoneNumber);

            var email = _context.Users.SingleOrDefault(u => u.PhoneNumber == phoneNumber).Email;
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var car = _context.Cars.SingleOrDefault(c => c.Id == carId);

            if (car == null)
            {
                return BadRequest("Car not found");
            }

            var price = car.Price * multiplier;

            // Create new purchase record
            var purchase = new Purchase
            {
                PhoneNumber = user.PhoneNumber,
                CarId = car.Id,
                Multiplier = multiplier
            };
            _context.Purchases.Add(purchase);
            _context.SaveChanges();


            var ticket = new
            {
                car.Id,
                car.Brand,
                car.Model,
                car.Year,
                car.ImageUrl1,
                car.ImageUrl2,
                car.ImageUrl3,
                car.Capacity,
                car.Transmission,
                car.CreatedBy,
                car.Price,
                car.Multiplier,
                car.FuelCapacity,
                car.City,
                car.CreatedByEmail,
                car.Latitude,
                car.Longitude,
                DayPrice = car.Price,
                PricePaid = price
            };



            return Ok(ticket);
        }


        [HttpGet("{phoneNumber}")]
        public IActionResult GetUserByPhoneNumber(string phoneNumber)
        {
            var purchases = _context.Purchases
                                    .Include(p => p.Car)
                                    .Where(p => p.PhoneNumber == phoneNumber);

            if (purchases == null || !purchases.Any())
            {
                return Ok("No products");
            }

            var purchasedCars = purchases.Select(purchase => new
            {
                CarId = purchase.Car.Id,
                CarBrand = purchase.Car.Brand,
                CarModel = purchase.Car.Model,
                City = purchase.Car.City,
                Multiplier = purchase.Multiplier,
                PricePaid = purchase.Multiplier * purchase.Car.Price
            });

            return Ok(purchasedCars);
        }





    }
}
