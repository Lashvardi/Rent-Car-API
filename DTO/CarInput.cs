namespace RentCar.DTO
{
    public class CarInputModel
    {
        // This Class Is Used For Inputting Data To The Database.
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public float Price { get; set; }
        public int Capacity { get; set; }
        public string Transmission { get; set; }
        public string CreatedBy { get; set; }
        public string City { get; set; }
        public string CreatedByEmail { get; set; }
        public int FuelCapacity { get; set; }
        public IFormFile Image1 { get; set; }
        public IFormFile Image2 { get; set; }
        public IFormFile Image3 { get; set; }

        // For Future Working With MapBox API (In Frontend)
        public double Latitude { get; set; } // Latitude property
        public double Longitude { get; set; } // Longitude property
    }
}
