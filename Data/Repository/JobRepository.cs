namespace Interimkantoor.Data.Repository
{
    public class JobRepository: GenericRepository<Job>, IJobRepository
    {
        public JobRepository(InterimkantoorContext context):base(context) { }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs.Include(x=>x.KlantJobs).ThenInclude(x=>x.Klant).ToListAsync();
        }

        public async Task<Job> GetJobAsync(int id)
        {
            return await _context.Jobs.Include(x => x.KlantJobs).ThenInclude(x => x.Klant).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<Job>> SearchJobsAsync(string zoekwaarde)
        {
            return await _context.Jobs
                        .Where(x => x.Omschrijving.Contains(zoekwaarde))
                        .Include(x => x.KlantJobs)
                        .ThenInclude(x => x.Klant)
                        .OrderBy(x => x.Omschrijving)
                        .ToListAsync();
        }

    }
}
