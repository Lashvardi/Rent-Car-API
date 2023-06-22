using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using RentCar.data;
using RentCar.DTO;
using RentCar.models;
using RentCar.Repository.Abstract;

namespace RentCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private readonly DataContext _context;
        private IFileService _fileService;

        public CarController(DataContext context, IFileService fs)
        {
            _context = context;
            _fileService = fs;
        }


        // Getting  Random 10 Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _context.Cars
                .OrderBy(c => Guid.NewGuid())
                .Take(12)
                .ToListAsync();

            if (cars == null || !cars.Any())
            {
                return NotFound();
            }

            return cars;
        }


        // Sending Get Request To Get All Cars (Paginated)
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginatedData<Car>>> GetPaginatedCars(int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                return BadRequest("Invalid page index.");
            }

            int totalItems = await _context.Cars.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageIndex > totalPages)
            {
                return BadRequest("Page index exceeds the total number of pages.");
            }

            var cars = await _context.Cars
                .OrderBy(c => c.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginatedData = new PaginatedData<Car>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Data = cars
            };

            return Ok(paginatedData);
        }


        // Sending Get Request To Get All Cars (Paginated, Filtered)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Car>>> FilterCars(int? capacity, int? startYear, int? endYear, string city, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Car> query = _context.Cars;

            if (capacity.HasValue)
            {
                query = query.Where(c => c.Capacity == capacity);
            }

            if (startYear.HasValue)
            {
                query = query.Where(c => c.Year >= startYear);
            }

            if (endYear.HasValue)
            {
                query = query.Where(c => c.Year <= endYear);
            }

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(c => c.City == city);
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageIndex < 1 || pageIndex > totalPages)
            {
                return BadRequest("Invalid page index.");
            }

            var cars = await query
                .OrderBy(c => c.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginatedData = new PaginatedData<Car>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Data = cars
            };

            return Ok(paginatedData);
        }



        // Sending Get Request To Get Popular Cars
        [HttpGet("popular")]
        public async Task<ActionResult<IEnumerable<Car>>> GetPopularCars()
        {
            var cars = await _context.Cars
                .OrderBy(c => Guid.NewGuid())
                .Take(4)
                .ToListAsync();

            if (cars == null || !cars.Any())
            {
                return NotFound();
            }

            return cars;
        }


        // Sending Get Request To Get Cars By Owner
        [HttpGet("byPhone")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsByOwner(string PhoneNumber)
        {
            var cars = await _context.Cars.Where(c => c.CreatedBy == PhoneNumber).ToListAsync();

            if (cars == null || !cars.Any())
            {
                return NotFound();
            }

            return cars;
        }

        // Sending Get Reqeust To Get All Cities.
        [HttpGet("cities")]
        public async Task<ActionResult<IEnumerable<string>>> GetCities()
        {
            var cities = await _context.Cars.Select(c => c.City).Distinct().ToListAsync();

            if (cities == null || !cities.Any())
            {
                return NotFound();
            }

            return cities;
        }

        // Sending Get Request To Get Car By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }


        // Sending Post Request To Create New Car
        // Will Implement Stack 
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar([FromForm] CarInputModel carInput)
        {
            var car = new Car
            {
                Brand = carInput.Brand,
                Model = carInput.Model,
                Year = carInput.Year,
                Price = carInput.Price,
                Capacity = carInput.Capacity,
                FuelCapacity = carInput.FuelCapacity,
                Transmission = carInput.Transmission,
                CreatedBy = carInput.CreatedBy,
                CreatedByEmail = carInput.CreatedByEmail,
                City = carInput.City,
                Latitude = carInput.Latitude,
                Longitude = carInput.Longitude,

            };

            // Check for null images and save them if provided
            if (carInput.Image1 != null)
            {
                var fileResult = _fileService.SaveImage(carInput.Image1);

                if (fileResult.Item1 == 1)
                {
                    car.ImageUrl1 = fileResult.Item2;
                }
            }

            if (carInput.Image2 != null)
            {
                var fileResult = _fileService.SaveImage(carInput.Image2);

                if (fileResult.Item1 == 1)
                {
                    car.ImageUrl2 = fileResult.Item2;
                }
            }

            if (carInput.Image3 != null)
            {
                var fileResult = _fileService.SaveImage(carInput.Image3);

                if (fileResult.Item1 == 1)
                {
                    car.ImageUrl3 = fileResult.Item2;
                }
            }

            int count = await _context.Cars.CountAsync();

            if (count >= 500)
            {
                var firstCar = await _context.Cars.Include(c => c.FavoritedByUsers).Include(c => c.Purchases).FirstOrDefaultAsync();

                if (firstCar != null)
                {
                    // Had to remove the related entities manually because of the foreign key constraint
                    _context.UserFavoriteCars.RemoveRange(firstCar.FavoritedByUsers);
                    _context.Purchases.RemoveRange(firstCar.Purchases);
                    _context.Cars.Remove(firstCar);
                }
            }

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
        }





    }
}