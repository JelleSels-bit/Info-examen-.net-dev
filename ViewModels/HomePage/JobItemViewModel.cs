namespace Interimkantoor.ViewModels.HomePage
{
    public class JobItemViewModel
    {
        public int Id { get; set; }
        public string Omschrijving { get; set; } = default!;
        public DateTime StartDatum { get; set; } = DateTime.Now;
        public DateTime EindDatum { get; set; } = DateTime.Now.AddDays(1);
        public string Locatie { get; set; } = default!;
        public bool IsWerkschoenen { get; set; }
        public bool IsBadge { get; set; }
        public bool IsKleding { get; set; }
        public int AantalPlaatsen { get; set; }
        public int VrijePlaatsen { get; set; }
    }
}
