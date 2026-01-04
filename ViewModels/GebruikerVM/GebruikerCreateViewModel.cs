namespace Interimkantoor.ViewModels.GebruikerVM
{
    public class GebruikerCreateViewModel
    {
        [Required]
        public string GebruikersNaam { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public string Voornaam { get; set; }
        [Required]

        public DateTime Geboortedatum { get; set; }
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public SelectList? Rollen { get; set; }
        public string RolId { get; set; }
    }
}
