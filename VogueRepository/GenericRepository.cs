using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities;
using VogueCore.Repositories;
using VogueCore.Specifications;
using VogueRepository.Data;

namespace VogueRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region WithOut Specefications
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);

            // return await _dbContext.Products.Where(P=>P.Id ==id).Include(P => P.ProductBrand).Include(P => P.ProductType).FirstOrDefaultAsync(); 
        }
        #endregion

        #region With Specifications
            public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
            {
                return await ApplySpecification(Spec).ToListAsync();
            }

            public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec)
            {
                return await ApplySpecification(Spec).FirstOrDefaultAsync();
            }

            private IQueryable<T> ApplySpecification(ISpecifications<T> Spec)
            {
                return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), Spec);
            }

        #endregion

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).CountAsync();
        }

        public async Task Add(T item)
        =>await _dbContext.Set<T>().AddAsync(item);

        public void Delete(T item)
        => _dbContext.Set<T>().Remove(item);

        public void Update(T item)
        => _dbContext.Set<T>().Update(item);

        public async Task AddAsync(T entity)
         => await _dbContext.Set<T>().AddAsync(entity);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await _dbContext.Set<T>()
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(ISpecifications<T> spec)
        {
            // استخدام SpecificationEvaluator لبناء الاستعلام بناءً على المواصفات
            var query = SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
            return await query.CountAsync();
        }
    }
}
