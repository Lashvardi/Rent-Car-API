using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentCar.data;
using RentCar.DTO;
using RentCar.models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using SMTP;
namespace RentCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly SendMessage _sendMEssage;


        public UsersController(DataContext context, IConfiguration configuration, SendMessage sendMEssage)
        {
            _context = context;
            _configuration = configuration;
            _sendMEssage = sendMEssage;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var existingUser = await _context.Users.FindAsync(request.PhoneNumber);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            var user = new User
            {
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = "User"
            };
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            await _sendMEssage.SendRegistrationEmail(request.Email, request.FirstName, request.LastName);

            return Ok(user);
        }

            [HttpPost("login")]
            public async Task<ActionResult> Login(UserDTO request)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
                if (user == null)
                {
                    return BadRequest("User Not Found!");
                }

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest("Wrong Password");
                }

                var token = CreateToken(user);
                return Ok(new { token, user.FirstName, user.LastName, user.Role, user.PhoneNumber, user.Email });
            }

        [HttpGet("{phoneNumber}/favorite-cars")]
        public IActionResult GetFavoriteCars(string phoneNumber)
        {
            var user = _context.Users.Include(u => u.FavoriteCars)
                                     .ThenInclude(ufc => ufc.Car)
                                     .FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var favoriteCars = user.FavoriteCars.Select(ufc => ufc.Car);

            return Ok(favoriteCars);
        }



        [HttpPost("{userId}/favorites/{carId}")]
        public async Task<ActionResult> AddToFavorites(string userId, int carId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return NotFound("Car not found");
            }

            var userFavoriteCar = new UserFavoriteCar
            {
                UserId = userId,
                CarId = carId
            };

            await _context.UserFavoriteCars.AddAsync(userFavoriteCar);
            await _context.SaveChangesAsync();

            return Ok();
        }


        private string CreateToken(User user)
        {
            // Create a list of claims for the token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };
            // Retrieve the token key from configuration and convert it to a symmetric security key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Create a new JWT security token
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);
           
            // Generate the token string from the token object
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpGet("{phoneNumber}")]
        public async Task<ActionResult<User>> GetUser(string phoneNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}