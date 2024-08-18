using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Parkomat.Models
{
    /// <summary>
    /// Represents a parking session with details such as car license plate, timing, and cost.
    /// </summary>
    public class Parking
    {
        /// <summary>
        /// Gets or sets the unique identifier for the parking session.
        /// </summary>
        [Key]
        public int ParkingId { get; set; }

        /// <summary>
        /// Gets or sets the car license plate associated with the parking session.
        /// </summary>
        public string? CarLicensePlate { get; set; }

        /// <summary>
        /// Gets or sets the start time of the parking session.
        /// </summary>
        public DateTime? ParkingStart { get; set; }

        /// <summary>
        /// Gets or sets the stop time of the parking session. Can be null if the session is ongoing.
        /// </summary>
        [AllowNull]
        public DateTime? ParkingStop { get; set; }

        /// <summary>
        /// Gets or sets the cost of the parking session. Nullable to allow for calculations before setting a value.
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        [AllowNull]
        public decimal? Cost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parking session has been paid.
        /// </summary>
        public bool? Payed { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parking lot where the session took place.
        /// </summary>
        public int ParkingLotID { get; set; }

        /// <summary>
        /// Gets or sets the parking lot associated with the parking session.
        /// </summary>
        public ParkingLot? ParkingLot { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the parking session.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the parking session.
        /// </summary>
        public ApplicationUser? User { get; set; }
    }
}
