using System.Threading.Tasks;

namespace OrionsCloud.Backend.Cache
{
    public interface ICache<T> 
    {
        Task CreateAsync(string key, T item);

        Task CreateAsync(string key, T item, string transactionId);

        Task UpdateAsync(string key, T item);

        Task UpdateAsync(string key, T item, string transactionId);

        Task<T> GetAsync(string key);

        Task DeleteAsync(string key);

        Task DeleteAsync(string key, string transactionId);
    }
}
