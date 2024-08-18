using Microsoft.AspNetCore.Identity;

namespace Parkomat.Models
{
    /// <summary>
    /// Represents an application user with additional profile data.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname (last name) of the user.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the birthday of the user.
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Gets or sets the collection of parkings associated with the user.
        /// </summary>
        public ICollection<Parking>? Parkings { get; set; }
    }
}
