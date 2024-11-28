using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities;
using VogueCore.Specifications;

namespace VogueRepository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {

        //_dbContext.Set<T>().Where(P=>P.Id ==id).Include(P => P.ProductBrand).Include(P => P.ProductType)

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,ISpecifications<T> Spec)
        {
            var Query = inputQuery; //_dbContext.Set<T>()
            if(Spec.Criteria is not null)
            {
                Query = Query.Where(Spec.Criteria);//_dbContext.Set<T>().Where(P=>P.Id ==id)
            }

            if(Spec.OrderBy is not null)
            {
                Query = Query.OrderBy(Spec.OrderBy);   
            }

            if (Spec.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(Spec.OrderByDescending);
            }

            if (Spec.IsPaginationEnabled)
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);
            }

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            return Query;
        }

    }
}
