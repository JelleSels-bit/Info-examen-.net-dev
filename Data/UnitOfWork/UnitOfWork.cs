
namespace Interimkantoor.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InterimkantoorContext _context;

        private IKlantRepository klantRepository;
        private IJobRepository jobRepository;
        private IGenericRepository<KlantJob> klantJobRepository;

        public UnitOfWork(InterimkantoorContext context)
        {
            _context = context;
        }

        public IKlantRepository KlantRepository
        {
            get
            {
                return klantRepository ??= new KlantRepository(_context);
            }
        }            
        
        public IGenericRepository<KlantJob> KlantJobRepository
        {
            get
            {
                return klantJobRepository ??= new GenericRepository<KlantJob>(_context);
            }
        }       
        public  IJobRepository JobRepository
        {
            get
            {
                return jobRepository ??= new JobRepository(_context);
            }
        }        

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
