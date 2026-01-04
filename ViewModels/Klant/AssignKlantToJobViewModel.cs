namespace Interimkantoor.ViewModels.Klant
{
    public class AssignKlantToJobViewModel
    {
        [Required(ErrorMessage = "Selecteer een klant")]
        public string KlantId { get; set; }
        public int JobId { get; set; }

        public string? JobOmschrijving { get; set; }

        public List<SelectListItem> Klanten = new();
    }
}
