using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IDuplicationRepository<T>where T : class
    {
        Task<bool> IsDuplicate(T entity, int? id);
    }
}
