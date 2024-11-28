using AutoMapper;
using Vogue.Dashboard.Models;
using VogueCore.Entities;

namespace Vogue.Dashboard.Helpers
{
    public class MapsProfile:Profile
    {
        public MapsProfile()
        {
            CreateMap<Product,ProductViewModel>().ReverseMap();
        }
    }
}
