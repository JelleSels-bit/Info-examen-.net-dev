namespace Interimkantoor.Data.Repository
{
    public class KlantRepository: GenericRepository<Klant>, IKlantRepository
    {
        public KlantRepository(InterimkantoorContext context) : base(context) { }

        public async Task<IList<Klant>> SearchKlantAsync(Expression<Func<Klant, bool>>? zoekwaarde)
        {
            return await _context.Klanten
                        .Where(zoekwaarde)
                        .OrderByDescending(x => x.Naam)
                        .ToListAsync();
        }
    }
}
