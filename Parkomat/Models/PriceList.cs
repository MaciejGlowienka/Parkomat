using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkomat.Models
{
    /// <summary>
    /// Represents a price list for parking with rates for different time periods.
    /// </summary>
    public class PriceList
    {
        /// <summary>
        /// Gets or sets the unique identifier for the price list.
        /// </summary>
        [Key]
        public int PriceListId { get; set; }

        /// <summary>
        /// Gets or sets the price for the first hour of parking.
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Hour1 { get; set; }

        /// <summary>
        /// Gets or sets the price for the second hour of parking.
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Hour2 { get; set; }

        /// <summary>
        /// Gets or sets the price for the third hour of parking.
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Hour3 { get; set; }

        /// <summary>
        /// Gets or sets the price for each additional hour after the third hour.
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Rest { get; set; }

        /// <summary>
        /// Gets or sets the collection of parking lots that use this price list.
        /// </summary>
        public ICollection<ParkingLot>? ParkingLots { get; set; }
    }
}
