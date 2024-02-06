using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICrudRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<T> GetLast();
        Task<IEnumerable<T>> GetAll();
        Task<OperationResult> Add(T entity);
        Task<OperationResult> Update(int id, T entity);
        Task Remove(T entity);
    }
}
