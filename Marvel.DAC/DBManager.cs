using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.DAC
{
    public class DBManager
    {

        /// <summary>
        /// Gets the database factory.
        /// </summary>
        protected DbProviderFactory DbFactory { get; private set; }

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        protected string DbConnectionString { get; private set; }

        public DBManager()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["DBConnectionString"];
            DbConnectionString = connectionStringSettings.ConnectionString;
            DbFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
        }
    }
}
