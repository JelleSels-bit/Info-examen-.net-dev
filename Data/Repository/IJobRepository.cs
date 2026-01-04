namespace Interimkantoor.Data.Repository
{
    public interface IJobRepository: IGenericRepository<Job>
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();

        Task<Job> GetJobAsync(int id);
        Task<IList<Job>> SearchJobsAsync(string zoekwaarde);
    }
}
