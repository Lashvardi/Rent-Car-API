using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.data;
using RentCar.models;
namespace RentCar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        private readonly DataContext _context;

        public MessageController(DataContext context)
        {
            _context = context;
        }
        // Sending Post Request To Send Message
        // Gets PhoneNumber/CarId
        [HttpPost("Message")]
        public IActionResult SendMessage(string phoneNumber, int CarId)
        {
            var user = _context.Users.Include(u => u.Message)
                                      .SingleOrDefault(u => u.PhoneNumber == phoneNumber);
            var car = _context.Cars.SingleOrDefault(c => c.Id == CarId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var random = new Random();
            var messages = new List<string>
        {
            $"Your Car  {car.Brand}/{car.Model} Has Ben Rented",
            $"Hey Someone Rented Your {car.Brand}/{car.Model}",
            $"{car.Brand}/{car.Model} Has Been Rented"
        };
            var randomMessage = new Message { MessageText = messages[random.Next(messages.Count)] };

            user.Message.Add(randomMessage);

            _context.SaveChanges();

            return Ok(randomMessage);
        }

        // Sending Get Request To Get Message
        // Gets PhoneNumber
        [HttpGet("Messages")]
        public IActionResult GetMessages(string phoneNumber)
        {
            var user = _context.Users.Include(u => u.Message)
                                      .SingleOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var messages = user.Message.Select(m => m.MessageText).ToList();
            return Ok(messages);
        }
    }
}
