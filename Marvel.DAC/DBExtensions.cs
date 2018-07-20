// <copyright file="DbExtenstions.cs" company="SchoolSpeak Technologies Private Limited">
// Copyright (c) SchoolSpeak Technologies Private Limited. All rights reserved.
// </copyright>

using System.Data.Common;

namespace CB.IntegrationService.DAL
{
    /// <summary>
    /// Provides extension methods for Db operations
    /// </summary>
    internal static class DbExtenstions
    {
        /// <summary>
        /// Adds a Db parameter to the SQL command with value.
        /// </summary>
        /// <param name="dbCommand">The database command.</param>
        /// <param name="parameterName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        internal static void AddParameterWithValue(this DbCommand dbCommand, string parameterName, object fieldValue)
        {
            DbParameter parameter = dbCommand.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = fieldValue;
            dbCommand.Parameters.Add(parameter);
        }

        /// <summary>
        /// Creates a DbConnection using the provided factory and the specified connection string.
        /// </summary>
        /// <param name="dbFactory">The database factory.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>A DbConnection using the provided factory</returns>
        internal static DbConnection CreateConnection(this DbProviderFactory dbFactory, string connectionString)
        {
            DbConnection dbConnection = dbFactory.CreateConnection();
            dbConnection.ConnectionString = connectionString;

            return dbConnection;
        }
        
    }
}
