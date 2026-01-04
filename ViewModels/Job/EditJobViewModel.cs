namespace Interimkantoor.ViewModels.Job
{
    public class EditJobViewModel
    {
        public int Id { get; set; }
        public string Omschrijving { get; set; } = default!;
        public DateTime StartDatum { get; set; } = DateTime.Now;
        public DateTime EindDatum { get; set; } = DateTime.Now.AddDays(1);
        public string Locatie { get; set; } = default!;
        public bool IsWerkschoenen { get; set; }
        public bool IsBadge { get; set; }
        public bool IsKleding { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int AantalPlaatsen { get; set; }
        public List<KlantJobListViewModel>? KlantJobs { get; set; } = new List<KlantJobListViewModel>();
        public List<SelectListItem> Klanten { get; set; } = new List<SelectListItem>();

    }
}
