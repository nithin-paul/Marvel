using Marvel.DAC;
using Marvel.DAC.Interfaces;
using Marvel.Model;
using System;
using System.Collections.Generic;

namespace Marvel.BLL
{
    public class MarvelManageBLL
    {
        public ICategoryDAL CategoryManager = null;

        public ISliderDAL SliderManager = null;


        public MarvelManageBLL()
        {
            CategoryManager = new CategoryManagerDAC();
            SliderManager = new SliderManagerDAC();
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
    }
}
