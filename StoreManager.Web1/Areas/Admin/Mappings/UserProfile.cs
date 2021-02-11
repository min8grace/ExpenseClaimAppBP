using StoreManager.Infrastructure.Identity.Models;
using StoreManager.Web.Areas.Admin.Models;
using AutoMapper;

namespace StoreManager.Web.Areas.Admin.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}