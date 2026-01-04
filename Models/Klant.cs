



namespace Interimkantoor.Models
{
    public class Klant
    {
        [Required(ErrorMessage = "Id is verplicht.")]
        [RegularExpression(@"^[A-Z]{4}$", ErrorMessage = "Id moet bestaan uit 4 hoofdletters.")]
        public string Id { get; set; } = default!;

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam mag maximaal 100 karakters bevatten.")]
        public string Naam { get; set; } = default!;

        [Required(ErrorMessage = "Voornaam is verplicht.")]
        [StringLength(100, ErrorMessage = "Voornaam mag maximaal 100 karakters bevatten.")]
        public string Voornaam { get; set; } = default!;

        [Required(ErrorMessage = "Gemeente is verplicht.")]
        [StringLength(100, ErrorMessage = "Gemeente mag maximaal 100 karakters bevatten.")]
        public string Gemeente { get; set; } = default!;

        [Required(ErrorMessage = "Postcode is verplicht.")]
        public string Postcode { get; set; } = default!;

        [Required(ErrorMessage = "Straat is verplicht.")]
        [StringLength(100, ErrorMessage = "Straat mag maximaal 100 karakters bevatten.")]
        public string Straat { get; set; } = default!;

        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        [StringLength(10, ErrorMessage = "Huisnummer mag maximaal 10 karakters bevatten.")]
        public string Huisnummer { get; set; } = default!;

        [Required(ErrorMessage = "Bankrekeningnummer is verplicht.")]
        public string Bankrekeningnummer { get; set; } = default!;

        [JsonIgnore]
        public List<KlantJob>? KlantJobs { get; set; }
    }
}
