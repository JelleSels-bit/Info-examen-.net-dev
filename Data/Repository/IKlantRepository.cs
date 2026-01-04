namespace Interimkantoor.Data.Repository
{
    public interface IKlantRepository: IGenericRepository<Klant>
    {
        Task<IList<Klant>> SearchKlantAsync(Expression<Func<Klant, bool>>? zoekwaarde);
    }
}
