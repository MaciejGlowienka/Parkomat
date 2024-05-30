using Microsoft.AspNetCore.Identity;

namespace Parkomat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Parking>? Parkings { get; set; }
    }
}
