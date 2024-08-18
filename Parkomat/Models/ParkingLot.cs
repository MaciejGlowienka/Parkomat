using System.ComponentModel.DataAnnotations;

namespace Parkomat.Models
{
    /// <summary>
    /// Represents a parking lot with details such as name and associated price list.
    /// </summary>
    public class ParkingLot
    {
        /// <summary>
        /// Gets or sets the unique identifier for the parking lot.
        /// </summary>
        [Key]
        public int ParkingLotId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parking lot.
        /// </summary>
        [Required]
        public string ParkingLotName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the price list associated with the parking lot.
        /// </summary>
        public int PriceListId { get; set; }

        /// <summary>
        /// Gets or sets the price list associated with the parking lot.
        /// </summary>
        public PriceList PriceList { get; set; }

        /// <summary>
        /// Gets or sets the collection of parking sessions associated with the parking lot.
        /// </summary>
        public ICollection<Parking> Parkings { get; set; }
    }
}
