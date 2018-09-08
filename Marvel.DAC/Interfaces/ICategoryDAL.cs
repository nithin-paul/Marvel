using Marvel.Model;
using System.Collections.Generic;

namespace Marvel.DAC.Interfaces
{
    public interface ICategoryDAL
    {
        CategoryDTO GetCategory(int catId);

        CategoryDTO AddOrUpdateCategory(int? catId, CategoryDTO category);

        List<CategoryDTO> GetCategories();

        List<ItemDTO> GetItemsByCategory(int catId);

        ItemDTO GetItem(int itemId);

        ItemDTO AddOrUpdateItem(int? itemId, ItemDTO item);

        bool DeleteImage(int id);

        bool SetImageAsPrimary(int id, int itemId);

        bool DeleteItem(int id);

        bool DeleteCategory(int id);
    }
}
