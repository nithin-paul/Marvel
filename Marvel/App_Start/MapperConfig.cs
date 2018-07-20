using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Marvel.App_Start
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                // Map from MessageSummaryDTO to MessageSummary
                //cfg.CreateMap<MessageSummaryDTO, MessageSummaryVM>();
            });
        }
    }
}