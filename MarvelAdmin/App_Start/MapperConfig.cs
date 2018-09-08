using AutoMapper;
using Marvel.Model;
using Marvel.Utils;
using MarvelAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarvelAdmin
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserVM>().ReverseMap();
                cfg.CreateMap<CategoryDTO, CategoryVM>().ForMember(x => x.Image, opt => opt.Ignore()).ReverseMap();
                cfg.CreateMap<ItemDTO, ItemVM>().ForMember(x => x.Image, opt => opt.Ignore()).ReverseMap();
                cfg.CreateMap<SliderDTO, SliderVM>().ForMember(x => x.Image, opt => opt.Ignore()).ReverseMap();
            });
            MapperManager.RegisterMappings(mapperConfiguration);
        }
    }
}