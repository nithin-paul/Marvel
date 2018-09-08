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
using System.Web;
using System.IO;

namespace Marvel.DAC
{
    public class SliderManagerDAC : DBManager, ISliderDAL
    {
        public bool DeleteSlider(int id)
        {
            try
            {
                // Get the images and remove upon successfull deletion
                SliderDTO slider = GetSlider(id);
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from slider_images where id=@Id";
                    cmd.AddParameter("@Id", id, DbType.Int32);
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                    if (slider.ImageUrl != null)
                    {
                        var filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + slider.ImageUrl);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SliderDTO AddOrUpdateSlider(int? Id, SliderDTO slider)
        {
            try
            {
                SliderDTO sliderDTO = null;
                if (Id != null)
                {
                    sliderDTO = GetSlider((int)Id);
                }
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CreateUpdateSlider";
                    cmd.AddParameter("@Id", Id, DbType.Int32);
                    cmd.AddParameterWithValue("@Name", slider.ImageUrl);
                    cmd.AddParameterWithValue("@Title", slider.Title); 
                    cmd.AddParameterWithValue("@Description", slider.Description);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SliderDTO sliderDto = new SliderDTO
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                ImageUrl = reader["image_name"].ToString(),
                                Title = reader["title"].ToString(),
                                Description = reader["description"].ToString()
                            };
                            if (Id != null)
                            {
                                if (sliderDTO.ImageUrl != slider.ImageUrl)
                                {
                                    var filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + sliderDTO.ImageUrl);
                                    if (File.Exists(filePath))
                                    {
                                        File.Delete(filePath);
                                    }
                                }
                            }
                            return sliderDto;
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

        public List<SliderDTO> GetSliders()
        {
            List<SliderDTO> sliders = null;
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = "SELECT id,image_name,title, description FROM [slider_images]";
                    cmd.CommandType = CommandType.Text;
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        sliders = new List<SliderDTO>();
                        while (reader.Read())
                        {
                            SliderDTO sliderDto = new SliderDTO
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                ImageUrl = reader["image_name"].ToString(),
                                Title = reader["title"].ToString(),
                                Description = reader["description"].ToString()
                            };
                            sliders.Add(sliderDto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sliders;
        }

        public bool DeleteImage(int id)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = $"DELETE from images where id=@id";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter("@id", id, DbType.Int32);
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public SliderDTO GetSlider(int id)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = "SELECT id,image_name,title, description FROM [slider_images] where id=@Id";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter("@Id", id, DbType.Int32);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SliderDTO sliderDto = new SliderDTO
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                ImageUrl = reader["image_name"].ToString(),
                                Title = reader["title"].ToString(),
                                Description = reader["description"].ToString()
                            };
                            return sliderDto;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
