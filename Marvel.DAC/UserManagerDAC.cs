using Marvel.DAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marvel.Model;
using System.Data.Common;
using CB.IntegrationService.DAL;
using System.Data;
using Marvel.Utils;

namespace Marvel.DAC
{
    public class UserManagerDAC : DBManager, IUserDAL
    {
        public UserDTO GetUser(long userId)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = "SELECT id,user_name,password,role FROM user WHERE id = @userId";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameterWithValue("@userId", userId);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDTO userDto = new UserDTO
                            {
                                Id = long.Parse(reader["Id"].ToString()),
                                UserName = reader["user_name"].ToString(),
                                Password = reader["password"].ToString(),
                                Role = Utilities.Split(reader["role"].ToString(), ", ", "[]").ToList()
                            };
                            return userDto;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public UserDTO ValidateLoginRequest(string userName , string password)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = "SELECT id,user_name,password,role FROM user WHERE user_name = @userName AND password= @password";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameterWithValue("@userName", userName);
                    cmd.AddParameterWithValue("@password", password);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDTO userDto = new UserDTO
                            {
                                Id = long.Parse(reader["Id"].ToString()),
                                UserName = reader["user_name"].ToString(),
                                Password = reader["password"].ToString(),
                                Role = Utilities.Split(reader["role"].ToString(), ", ", "[]").ToList()
                            };
                            return userDto;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
