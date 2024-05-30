using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkomat.Models
{
    public class Parking
    {
        [Key]
        public int ParkingId { get; set; }

        [Required]
        public string? CarLicensePlate { get; set; }
        public DateTime? ParkingStart { get; set; }
        public DateTime? ParkingStop { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Cost { get; set; }
        public int ParkingLotID { get; set; }
        public ParkingLot? ParkingLot { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
