using AutoMapper;
using Rectorix.Application.DTOs.Auth;
using Rectorix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Application
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode,
                    Country = src.Country
                }));

            // Optionally, you can reverse map (User to UserRegisterDto) if needed:
            // CreateMap<User, UserRegisterDto>();
        }
    }
}
