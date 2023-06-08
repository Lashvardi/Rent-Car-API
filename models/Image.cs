using RentCar.models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}