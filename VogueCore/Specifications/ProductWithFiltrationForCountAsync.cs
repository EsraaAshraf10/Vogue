using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities;

namespace VogueCore.Specifications
{
    public class ProductWithFiltrationForCountAsync :BaseSpecifications<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParams Params) 
            :base(P=>
                (string.IsNullOrEmpty(Params.Search) || P.Name.Contains(Params.Search))
                &&
                (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
                &&
                (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
            
        }
    }
}
