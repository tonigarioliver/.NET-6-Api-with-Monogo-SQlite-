using AutoMapper;
using LearnApi.Entity;

namespace LearnApi.Models.Profiles
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>().ForMember(item => item.Statusname, opt => opt.MapFrom(
                item => (item.IsActive != null && item.IsActive.Value) ? "Active" : "In active")).ReverseMap();
        }
    }
}
