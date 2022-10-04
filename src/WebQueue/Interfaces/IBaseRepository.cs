using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebQueue.Interfaces
{
    public interface IBaseRepository<T>
    {
        void Save(T item);
        Task<List<T>> ToListAsync();
    }
}
