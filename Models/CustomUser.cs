using Microsoft.AspNetCore.Identity;

namespace Interimkantoor.Models
{
    public class CustomUser:IdentityUser
    {
        [PersonalData]
        [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
        public string Voornaam { get; set; }

        [PersonalData]
        [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
        public string Achternaam { get; set; }

        [PersonalData]
        public Geslacht GeslachtSelect { get; set; }

        [PersonalData]
        [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
        public string? Straat { get; set; }

        [PersonalData]
        public int? Huisnummer { get; set; }

        [PersonalData]
        public string? Postcode { get; set; }

        [PersonalData]
        [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
        public string? Gemeente { get; set; }

        [PersonalData]
        public DateTime Geboortedatum { get; set; }

    }
}
