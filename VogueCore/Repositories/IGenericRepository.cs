using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities;
using VogueCore.Specifications;

namespace VogueCore.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specefications
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec);
        #endregion

        //Count
        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);

        Task AddAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        //Pagination
        Task<IReadOnlyList<T>> GetAllAsync(int pageIndex, int pageSize); // for pagination
        Task<int> CountAsync(); // for pagination
        Task<int> CountAsync(ISpecifications<T> spec);
        Task Add(T item);
        void Delete(T item);
        void Update(T item);
    }
}
