using Marvel.DAC;
using Marvel.DAC.Interfaces;
using Marvel.Model;
using System;
using System.Collections.Generic;

namespace Marvel.BLL
{
    public class AdminManageBLL
    {
        public UserManagerDAC userManager = null;

        public ICategoryDAL CategoryManager = null;

        public ISliderDAL SliderManager = null;


        public AdminManageBLL()
        {
            userManager = new UserManagerDAC();
            CategoryManager = new CategoryManagerDAC();
            SliderManager = new SliderManagerDAC();
        }

        public UserDTO AuthenticateUser(string userName, string password)
        {
            try
            {
                UserDTO userDto = userManager.ValidateLoginRequest(userName, password);
                return userDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ItemDTO> GetItemsByCategory (int catId)
        {
            try
            {
                return CategoryManager.GetItemsByCategory(catId);
            }
            catch
            {
                return null;
            }
        }

        public ItemDTO GetItemById(int itemId)
        {
            try
            {
                return CategoryManager.GetItem(itemId);
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteItem(int id)
        {
            try
            {
                return CategoryManager.DeleteItem(id);
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            try
            {
                return CategoryManager.DeleteCategory(id);
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSlider(int id)
        {
            try
            {
                return SliderManager.DeleteSlider(id);
            }
            catch
            {
                return false;
            }
        }

        public CategoryDTO GetCategoryById(int catId)
        {
            try
            {
                CategoryDTO catDto = CategoryManager.GetCategory(catId);
                return catDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SliderDTO GetSliderById(int sliderId)
        {
            try
            {
                SliderDTO sliderDTO = SliderManager.GetSlider(sliderId);
                return sliderDTO;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SliderDTO> GetSliders()
        {
            try
            {
                List<SliderDTO>  sliderDTO = SliderManager.GetSliders();
                return sliderDTO;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<CategoryDTO> GetCategories()
        {
            try
            {
                List<CategoryDTO> catDto = CategoryManager.GetCategories();
                return catDto;
            }
            catch
            {
                return null;
            }
        }

        public CategoryDTO AddOrUpdateCategory(int? catId,CategoryDTO category)
        {
            try
            {
                CategoryDTO catDto = CategoryManager.AddOrUpdateCategory(catId,category);
                return catDto;
            }
            catch
            {
                return null;
            }
        }

        public ItemDTO AddOrUpdateItem(int? itemId, ItemDTO item)
        {
            try
            {
                ItemDTO itemDto = CategoryManager.AddOrUpdateItem(itemId, item);
                return itemDto;
            }
            catch
            {
                return null;
            }
        }

        public SliderDTO AddOrUpdateSlider(int? id, SliderDTO slider)
        {
            try
            {
                SliderDTO sliderDto = SliderManager.AddOrUpdateSlider(id, slider);
                return sliderDto;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteImage(int id)
        {
            try
            {
                if (CategoryManager.DeleteImage(id))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool SetImageAsPrimary(int id, int itemId)
        {
            try
            {
                if (CategoryManager.SetImageAsPrimary(id,itemId))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
