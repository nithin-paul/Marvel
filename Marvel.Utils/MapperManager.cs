using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Utils
{
    public class MapperManager
    {  /// <summary>
       /// The mapper configuration
       /// </summary>
        private static MapperConfiguration mapperConfiguration;

        /// <summary>
        /// The mapper factory
        /// </summary>
        private static IMapper mapperFactory;

        /// <summary>
        /// Gets the mapper instance.
        /// </summary>
        /// <returns>return the IMapper instance</returns>
        public static IMapper GetMapperInstance() => mapperFactory;

        /// <summary>
        /// Registers the mappings.
        /// </summary>
        /// <param name="mapperConfig">The mapper configuration.</param>
        public static void RegisterMappings(MapperConfiguration mapperConfig = null)
        {
            if (mapperConfig != null)
            {
                mapperConfiguration = mapperConfig;
            }

            mapperFactory = mapperConfiguration.CreateMapper();
        }        
    }
}
