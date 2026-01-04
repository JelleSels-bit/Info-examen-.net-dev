

namespace Interimkantoor.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IKlantRepository KlantRepository { get; }
	    IJobRepository JobRepository { get;  }

        IGenericRepository<KlantJob> KlantJobRepository { get; }

	    public Task SaveChangesAsync();
    }
}
