using Marvel.DAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Marvel.Model;
using System.Data.Common;
using CB.IntegrationService.DAL;
using System.Data;
using Marvel.Utils;
using System.Web;

namespace Marvel.DAC
{
    public class CategoryManagerDAC : DBManager, ICategoryDAL
    {
        public List<ItemDTO> GetItemsByCategory(int catId)
        {
            List<ItemDTO> items = new List<ItemDTO>();
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = $"Select " +
                                                $"i.id As ItemId, " +
                                                $"i.cat_id As CategoryId, " +
                                                $"i.name as ItemName," +
                                                $"i.offer_percent as OfferPercent," +
                                                $"i.description as ItemDescription," +
                                                $"i.detail_description as DetailDescription," +
                                                $"i.price as ItemPrice," +
                                                $"img.id as ImageId," +
                                                $"img.item_id as ImageItemId," +
                                                $"img.name as ImageName," +
                                                $"img.is_primary as IsImagePrimary " +
                                        $"from items i left join images img on i.id = img.item_id where i.cat_id = @catId order by i.id ASC";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter("@catId", catId, DbType.Int32);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int itemId = Convert.ToInt32(reader["ItemId"].ToString());
                            ItemDTO item = items.FirstOrDefault(x => x.Id.Equals(itemId));
                            if (item == null)
                            {
                                item = new ItemDTO();
                                item.Id = itemId;
                                item.CategoryId = Convert.ToInt32(reader["CategoryId"].ToString());
                                item.Name = reader["ItemName"].ToString();
                                item.OfferPercent = Convert.ToDouble(reader["OfferPercent"].ToString());
                                item.Description = reader["ItemDescription"].ToString();
                                item.DetailDescription = reader["DetailDescription"].ToString();
                                item.Price = Convert.ToDouble(reader["ItemPrice"].ToString());
                                item.Images = new List<ImageModel>();
                                items.Add(item);
                            }
                            if (reader["ImageId"] != DBNull.Value)
                            {
                                ImageModel img = new ImageModel();
                                img.Id = Convert.ToInt32(reader["ImageId"].ToString());
                                img.ItemId = Convert.ToInt32(reader["ImageItemId"].ToString());
                                img.Name = reader["ImageName"].ToString();
                                img.IsPrimary = (Convert.ToInt32(reader["IsImagePrimary"].ToString()) == 1) ? true : false;
                                item.Images.Add(img);
                            }
                        }
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteItem(int id)
        {
            try
            {
                ItemDTO item= GetItem(id);
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "DeleteItem";
                    cmd.AddParameter("@Id", id, DbType.Int32);
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                    //Delete the images
                    if (item.Images != null)
                    {
                        foreach (ImageModel img in item.Images)
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + img.Name);
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }

                            filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(img.Name) + "_thumb" + Path.GetExtension(img.Name));
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(img.Name) + "_detail" + Path.GetExtension(img.Name));
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
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

        public bool DeleteCategory(int id)
        {
            try
            {
                //Get category
                CategoryDTO category = GetCategory(id);
                //Get Items
                List<ItemDTO> items = GetItemsByCategory(id);
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "DeleteCategory";
                    cmd.AddParameter("@Id", id, DbType.Int32);
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                    var filePath = string.Empty;
                    if (items != null)
                    {
                        foreach (ItemDTO item in items)
                        {
                            if (item.Images == null)
                                continue;
                            foreach (ImageModel img in item.Images)
                            {
                                filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + img.Name);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }

                                filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(img.Name) + "_thumb" + Path.GetExtension(img.Name));
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(img.Name) + "_detail" + Path.GetExtension(img.Name));
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                            }
                        }
                    }
                    filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + category.ImageUrl);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CategoryDTO AddOrUpdateCategory(int? catId, CategoryDTO category)
        {
            try
            {
                CategoryDTO catDTO = null;
                if (catId != null)
                {
                    catDTO = GetCategory((int)catId);
                }
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CreateUpdateCategory";
                    cmd.AddParameter("@Id", catId, DbType.Int32);
                    cmd.AddParameter("@Oid", category.Oid, DbType.Int32);
                    cmd.AddParameterWithValue("@Name", category.Name);
                    cmd.AddParameterWithValue("@Description", category.Description);
                    cmd.AddParameter("@ImageUrl", category.ImageUrl, DbType.String);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryDTO catDto = new CategoryDTO
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                Oid = Convert.ToInt32(reader["oid"].ToString()),
                                Name = reader["name"].ToString(),
                                ImageUrl = reader["image_url"].ToString(),
                                Description = reader["description"].ToString()
                            };
                            if (catDTO != null && category.ImageUrl != catDTO.ImageUrl)
                            {
                                var filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + catDTO.ImageUrl);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }

                                filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(catDTO.ImageUrl) + "_thumb" + Path.GetExtension(catDTO.ImageUrl));
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(catDTO.ImageUrl) + "_detail" + Path.GetExtension(catDTO.ImageUrl));
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                            }
                            return catDto;
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

        public ItemDTO AddOrUpdateItem(int? itemId, ItemDTO item)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "CreateUpdateItem";
                    cmd.AddParameter("@Id", itemId, DbType.Int32);
                    cmd.AddParameterWithValue("@catId", item.CategoryId);
                    cmd.AddParameterWithValue("@Name", item.Name);
                    cmd.AddParameterWithValue("@Description", item.Description);
                    cmd.AddParameterWithValue("@DetailDescription", item.DetailDescription);
                    cmd.AddParameterWithValue("@Price", item.Price);
                    cmd.AddParameterWithValue("@OfferPercent", item.OfferPercent);
                    cmd.AddParameter("@ImageName", item.ImageUrl,DbType.String);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        ItemDTO itemDto = new ItemDTO();
                        while (reader.Read())
                        {
                            itemDto.Id = Convert.ToInt32(reader["Id"].ToString());
                            itemDto.CategoryId = Convert.ToInt32(reader["CategoryId"].ToString());
                            itemDto.Name = reader["ItemName"].ToString();
                            itemDto.OfferPercent = Convert.ToDouble(reader["OfferPercent"].ToString());
                            itemDto.Description = reader["ItemDescription"].ToString();
                            itemDto.DetailDescription = reader["DetailDescription"].ToString();
                            itemDto.Price = Convert.ToDouble(reader["ItemPrice"].ToString());
                            if (itemDto.Images == null)
                            {
                                itemDto.Images = new List<ImageModel>();
                            }
                            if (reader["ImageId"] != DBNull.Value)
                            {
                                ImageModel img = new ImageModel();
                                img.Id = Convert.ToInt32(reader["ImageId"].ToString());
                                img.ItemId = Convert.ToInt32(reader["ImageItemId"].ToString());
                                img.Name = reader["ImageName"].ToString();
                                img.IsPrimary = (Convert.ToInt32(reader["IsImagePrimary"].ToString()) == 1) ? true : false;
                                itemDto.Images.Add(img);
                            }
                        }
                        return itemDto;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public List<CategoryDTO> GetCategories()
        {
            List<CategoryDTO> categories = null;
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = "SELECT id,oid,name,image_url, description FROM [categories]";
                    cmd.CommandType = CommandType.Text;
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        categories = new List<CategoryDTO>();
                        while (reader.Read())
                        {
                            CategoryDTO catDto = new CategoryDTO
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                Oid = Convert.ToInt32(reader["oid"].ToString()),
                                Name = reader["name"].ToString(),
                                ImageUrl = reader["image_url"].ToString(),
                                Description = reader["description"].ToString()
                            };
                            categories.Add(catDto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return categories;
        }

        public CategoryDTO GetCategory(int catId)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = "SELECT id,oid,name,image_url, description FROM [categories] WHERE id = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameterWithValue("@id", catId);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryDTO catDto = new CategoryDTO
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                Oid = Convert.ToInt32(reader["oid"].ToString()),
                                Name = reader["name"].ToString(),
                                ImageUrl = reader["image_url"].ToString(),
                                Description = reader["description"].ToString()
                            };
                            return catDto;
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

        public ItemDTO GetItem(int itemId)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = $"Select " +
                                                $"i.id As ItemId, " +
                                                $"i.cat_id As CategoryId, " +
                                                $"i.name as ItemName," +
                                                $"i.offer_percent as OfferPercent," +
                                                $"i.description as ItemDescription," +
                                                $"i.detail_description as DetailDescription," +
                                                $"i.price as ItemPrice," +
                                                $"img.id as ImageId," +
                                                $"img.item_id as ImageItemId," +
                                                $"img.name as ImageName," +
                                                $"img.is_primary as IsImagePrimary " +
                                        $"from items i left join images img on i.id = img.item_id where i.id = @itemId order by i.id ASC";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter("@itemId", itemId, DbType.Int32);
                    dbConnection.Open();

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        ItemDTO item = new ItemDTO();
                        while (reader.Read())
                        {
                            item.Id = itemId;
                            item.CategoryId = Convert.ToInt32(reader["CategoryId"].ToString());
                            item.Name = reader["ItemName"].ToString();
                            item.OfferPercent = Convert.ToDouble(reader["OfferPercent"].ToString());
                            item.Description = reader["ItemDescription"].ToString();
                            item.DetailDescription = reader["DetailDescription"].ToString();
                            item.Price = Convert.ToDouble(reader["ItemPrice"].ToString());
                            if (item.Images == null)
                            {
                                item.Images = new List<ImageModel>();
                            }
                            if (reader["ImageId"] != DBNull.Value)
                            {
                                ImageModel img = new ImageModel();
                                img.Id = Convert.ToInt32(reader["ImageId"].ToString());
                                img.ItemId = Convert.ToInt32(reader["ImageItemId"].ToString());
                                img.Name = reader["ImageName"].ToString();
                                img.IsPrimary = (Convert.ToInt32(reader["IsImagePrimary"].ToString()) == 1) ? true : false;
                                item.Images.Add(img);
                            }
                        }
                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public ImageModel GetImage(int id)
        {
            try
            {

                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = $"Select * from images where id=@id";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter("@id", id, DbType.Int32);
                    dbConnection.Open();
                    DataRow img = (DataRow)cmd.ExecuteScalar();
                    ImageModel imgMod = new ImageModel()
                    {
                        Id = id,
                        Name = img["Name"].ToString(),
                        IsPrimary = ((Convert.ToInt32(img["IsImagePrimary"].ToString()) == 1) ? true : false),
                        ItemId = Convert.ToInt32(img["item_id"])
                    };
                    return imgMod;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteImage(int id)
        {
            try
            {
                ImageModel imgModel = GetImage(id);
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandText = $"DELETE from images where id=@id";
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameter("@id", id, DbType.Int32);
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                    if (imgModel != null)
                    {
                        var filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + imgModel.Name);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(imgModel.Name) + "_thumb" + Path.GetExtension(imgModel.Name));
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        filePath = HttpContext.Current.Server.MapPath("~/Content/Images/" + Path.GetFileNameWithoutExtension(imgModel.Name) + "_detail" + Path.GetExtension(imgModel.Name));
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

        public bool SetImageAsPrimary(int id, int itemId)
        {
            try
            {
                using (DbConnection dbConnection = DbFactory.CreateConnection(DbConnectionString))
                {
                    DbCommand cmd = DbFactory.CreateCommand();
                    cmd.Connection = dbConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "SetImageAsPrimary";
                    cmd.AddParameter("@Id", id, DbType.Int32);
                    cmd.AddParameter("@ItemId", itemId, DbType.Int32);
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
    }
}
