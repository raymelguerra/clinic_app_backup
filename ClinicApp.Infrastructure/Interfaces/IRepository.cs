
namespace ClinicApp.Infrastructure.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        Task<T> GetByIdAsync<T>(int id) where T : class;
        Task<T> AddAsync<T>(T entity) where T : class;
        Task<T> UpdateAsync<T>(T entity) where T : class;
        Task<T> DeleteAsync<T>(T entity) where T : class;
    }
}
