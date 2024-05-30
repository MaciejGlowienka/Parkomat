using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkomat.Models
{
    public class PriceList
    {
        [Key]
        public int PriceListId { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Hour1 { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Hour2 { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Hour3 { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Rest { get; set; }

        public ICollection<ParkingLot>? ParkingLots { get; set; }
    }
}
