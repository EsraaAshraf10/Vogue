using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities;

namespace VogueCore.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        // _dbContext.Products.Where(P=>P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)

        //signature for property for where Condition [Where(P=>P.Id == id)]

        public Expression<Func<T , bool>> Criteria { get; set; }

        //signature for property for list of Icludes [Include(P => P.ProductBrand).Include(P => P.ProductType)]

        public List<Expression<Func<T,object>>> Includes { get; set; }

        // prop orderBy  [OrderBy(p=>p.Name)]
        public Expression<Func<T , object>> OrderBy { get; set; }

        // prop orderByDesc  [OrderByDesc(p=>p.Name)]
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        //Take
        public int Take { get; set; }

        //Skip
        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}
