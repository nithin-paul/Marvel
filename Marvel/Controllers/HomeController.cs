using Marvel.BLL;
using Marvel.Models;
using Marvel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marvel.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private MarvelManageBLL marvelManagerBll = null;
        public List<CategoryVM> Categories;
        public HomeController()
        {
            marvelManagerBll = new MarvelManageBLL();
            Categories = MapperManager.GetMapperInstance().Map<List<CategoryVM>>(marvelManagerBll.GetCategories());
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Slider()
        {
            // chane to slider images
            List<SliderVM> sliders = MapperManager.GetMapperInstance().Map<List<SliderVM>>(marvelManagerBll.GetSliders());
            return PartialView(sliders);
        }

        [HttpGet]
        [Route("Items/{catId}")]
        public ActionResult ItemListing(int catId)
        {
            ViewBag.CategoryName = String.Empty;
            if (Categories !=null)
            {
                var cat = Categories.FirstOrDefault(x => x.Id == catId);
                if (cat != null)
                {
                    ViewBag.CategoryName = cat.Name;
                    ViewBag.Description = cat.Description;
                }
            }
            
            List<ItemVM> items = MapperManager.GetMapperInstance().Map<List<ItemVM>>(marvelManagerBll.GetItemsByCategory(catId));
            return View(items);
        }

        [HttpGet]
        [Route("Item/{itemId}")]
        public ActionResult Details(int itemId)
        {
            ItemVM item = MapperManager.GetMapperInstance().Map<ItemVM>(marvelManagerBll.GetItemById(itemId));
            if (item != null)
            {
                var categoryDto = marvelManagerBll.GetCategoryById(item.CategoryId);
                if (categoryDto != null)
                {
                    ViewBag.categoryName = categoryDto.Name;
                }
            }
            return View(item);
        }

        public ActionResult RelatedItemListing(int id, int itemId)
        {
            List<ItemVM> items = MapperManager.GetMapperInstance().Map<List<ItemVM>>(marvelManagerBll.GetItemsByCategory(id));
            ItemVM item = items.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                items.Remove(item);
            }
            return PartialView(items);
        }

        public ActionResult CategoriesList()
        {
            return PartialView(Categories);
        }

        public ActionResult GetAllItems()
        {
            List<ItemVM> items = new List<ItemVM>();
            foreach (var cat in Categories)
            {
                List<ItemVM> itemForCat = MapperManager.GetMapperInstance().Map<List<ItemVM>>(marvelManagerBll.GetItemsByCategory(cat.Id));
                if (itemForCat.Count >0)
                {
                    items.AddRange(itemForCat);
                }
            }
            return PartialView(items);
        }

        public ActionResult CategoriesMenu()
        {
            return PartialView(Categories);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}