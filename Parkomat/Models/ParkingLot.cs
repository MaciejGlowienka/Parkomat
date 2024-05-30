using System.ComponentModel.DataAnnotations;

namespace Parkomat.Models
{
    public class ParkingLot
    {
        [Key]
        public int ParkingLotId { get; set; }
        [Required]
        public string ParkingLotName { get; set; }
        public int PriceListId { get; set; }
        public PriceList PriceList { get; set; }

        public ICollection<Parking> Parkings { get; set; }
    }
}
