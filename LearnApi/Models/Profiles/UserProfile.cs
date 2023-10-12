using AutoMapper;
using LearnApi.Entity;
using LearnApi.Models.DTO;

namespace LearnApi.Models.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile() { 
            CreateMap<UserRegisterDto,User>();
        }
    }
}

