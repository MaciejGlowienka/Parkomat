using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Parkomat.Models
{
    public class Parking
    {
        [Key]
        public int ParkingId { get; set; }
        public string? CarLicensePlate { get; set; }
        public DateTime? ParkingStart { get; set; }
        [AllowNull]
        public DateTime? ParkingStop { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [AllowNull]
        public decimal? Cost { get; set; }
        public int ParkingLotID { get; set; }
        public ParkingLot? ParkingLot { get; set; }
        
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
