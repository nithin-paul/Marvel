using AutoMapper;
using Marvel.Model;
using Marvel.Models;
using Marvel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marvel
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SliderDTO, SliderVM>().ForMember(x => x.Image, opt => opt.Ignore()).ReverseMap();
                cfg.CreateMap<CategoryDTO, CategoryVM>().ForMember(x => x.Image, opt => opt.Ignore()).ReverseMap();
                cfg.CreateMap<ItemDTO, ItemVM>().ForMember(x => x.Image, opt => opt.Ignore()).ReverseMap();
            });
            MapperManager.RegisterMappings(mapperConfiguration);
        }
    }
}