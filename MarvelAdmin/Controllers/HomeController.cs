using Marvel.BLL;
using Marvel.Model;
using Marvel.Utils;
using MarvelAdmin.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarvelAdmin
{
    [Authorize]
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private AdminManageBLL adminManagerBll = null;

        public List<CategoryVM> Categories;
        public HomeController()
        {
            adminManagerBll = new AdminManageBLL();
            Categories = MapperManager.GetMapperInstance().Map<List<CategoryVM>>(adminManagerBll.GetCategories());
        }

        public ActionResult Index()
        {
            List<CategoryVM> lstCat = new List<CategoryVM>();
            int id = Request.QueryString["Id"]!= null ? Convert.ToInt32( Request.QueryString["Id"].ToString()): 0;
            CategoryVM category = null;
            if (id > 0 && Categories != null)
            {
                lstCat.Add(Categories.FirstOrDefault(x => x.Id.Equals((int)id)));
            }
            else
            {
                lstCat.AddRange(Categories);
            }
            return View(lstCat);
        }

        public ActionResult CategorySideBar()
        {
            return PartialView(Categories);
        }

        public ActionResult SliderSideBar()
        {
            // chane to slider images
            List<SliderVM> sliders = MapperManager.GetMapperInstance().Map<List<SliderVM>>(adminManagerBll.GetSliders());
            return PartialView(sliders);
        }

        public ActionResult ItemListing(int id)
        {
            List<ItemVM> items = MapperManager.GetMapperInstance().Map<List<ItemVM>>(adminManagerBll.GetItemsByCategory(id));
            return PartialView(items);
        }

        public ActionResult ItemGridUpload(int id)
        {
            ItemVM items = MapperManager.GetMapperInstance().Map<ItemVM>(adminManagerBll.GetItemById(id));
            return PartialView(items);
        }
        public ActionResult RelatedItemListing(int id, int itemId)
        {
            List<ItemVM> items = MapperManager.GetMapperInstance().Map<List<ItemVM>>(adminManagerBll.GetItemsByCategory(id));
            ItemVM item = items.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                items.Remove(item);
            }
            return PartialView(items);
        }

        public ActionResult Details(int id)
        {
            // Get item By Id
            ItemVM item = MapperManager.GetMapperInstance().Map<ItemVM>(adminManagerBll.GetItemById(id));
            if (item != null)
            {
                var categoryDto = adminManagerBll.GetCategoryById(item.CategoryId);
                if (categoryDto != null)
                {
                    ViewBag.categoryName = categoryDto.Name;
                }
            }
            return View(item);
        }

        [HttpGet]
        [Route("Category/{Id?}")]
        public ActionResult CategoryAdd(int? id)
        {
            ViewBag.Id = id;
            ViewBag.ButtonName = "Save";
            CategoryVM category = null;
            if (id != null)
            {
                ViewBag.ButtonName = "Update";
                var categoryDto = adminManagerBll.GetCategoryById((int)id);
                if (categoryDto == null)
                {
                    TempData["Error"] = "Category not exists.";
                    return View(category);
                }
                category = MapperManager.GetMapperInstance().Map<CategoryVM>(categoryDto);
            }
            return View(category);
        }

        [HttpGet]
        [Route("Slider/{Id?}")]
        public ActionResult SliderAdd(int? id)
        {
            ViewBag.Id = id;
            ViewBag.ButtonName = "Save";
            SliderVM slider = null;
            if (id != null)
            {
                ViewBag.ButtonName = "Update";
                var SliderDto = adminManagerBll.GetSliderById((int)id);
                if (SliderDto == null)
                {
                    TempData["Error"] = "slider not exists.";
                    return View(slider);
                }
                slider = MapperManager.GetMapperInstance().Map<SliderVM>(SliderDto);
            }
            return View(slider);
        }

        [HttpPost]
        [Route("Slider/{Id?}")]
        public ActionResult SliderAdd(int? id, SliderVM slider)
        {
            ViewBag.Id = id;
            ViewBag.ButtonName = "Save";
            if (id != null)
            {
                ViewBag.ButtonName = "Update";
            }
            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            if (id == null && slider.Image == null)
            {
                TempData["Error"] = "Select a slider image.";
                return View(slider);
            }
            if (slider.Image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    slider.Image.InputStream.CopyTo(memoryStream);
                }
            }
            if (id != null)
            {
                var sliderDTO = adminManagerBll.GetSliderById((int)id);
                if (sliderDTO == null)
                {
                    TempData["Error"] = "Category not exists.";
                    return View(slider);
                }
                slider.ImageUrl = sliderDTO.ImageUrl;
            }
            if (slider.Image != null)
            {
                var filename = slider.Image.FileName;
                var filePathOriginal = Server.MapPath("/Content/Images/");
                string savedFileName = Path.Combine(filePathOriginal, filename);
                try
                {
                    slider.Image.SaveAs(savedFileName);

                    //Utilities.ResizeImage(Image.FromStream(slider.Image.InputStream, true, true), savedFileName);
                    slider.ImageUrl = slider.Image.FileName;
                }
                catch
                {
                    TempData["Error"] = "Failed to upload image try again";
                    return View(slider);
                }
            }
            var sldr = MapperManager.GetMapperInstance().Map<SliderDTO>(slider);
            var sldrDto = adminManagerBll.AddOrUpdateSlider(id, sldr);
            if (sldrDto == null)
            {
                TempData["Error"] = id != null ? "Updating the slider failed." : "Adding the slider failed";
                return View(slider);
            }
            TempData["Success"] = id != null ? "Updated the slider successfully." : "Successfully added the slider";
            ViewBag.Id = sldrDto.Id;
            return View(MapperManager.GetMapperInstance().Map<SliderVM>(sldrDto));
        }

        [HttpGet]
        [Route("Items/{Id?}")]
        public ActionResult ItemsAdd( int? id)
        {
            int catId = Convert.ToInt32(Request.QueryString["catId"]);
            ViewBag.Id = id;
            ViewBag.CatId = catId;
            ViewBag.ButtonName = "Save";
            ItemVM item = null;
            if (id != null)
            {
                ViewBag.ButtonName = "Update";
                var itemDto = adminManagerBll.GetItemById((int)id);
                if (itemDto == null)
                {
                    TempData["Error"] = "Item not exists.";
                    return View(item);
                }
                item = MapperManager.GetMapperInstance().Map<ItemVM>(itemDto);
            }
            return View(item);
        }

        [HttpPost]
        [Route("Items/{Id?}")]
        public ActionResult ItemsAdd( int? id, ItemVM item)
        {
            int catId = Convert.ToInt32(Request.QueryString["catId"]);
             ViewBag.Id = id;
            ViewBag.CatId = catId;
            ViewBag.ButtonName = "Save";
            if (id != null)
            {
                ViewBag.ButtonName = "Update";
            }
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            if (id == null && item.Image == null)
            {
                TempData["Error"] = "Select a slider image.";
                return View(item);
            }
            if (item.Image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    item.Image.InputStream.CopyTo(memoryStream);
                }
            }
            if (id != null)
            {
                var itemDto = adminManagerBll.GetItemById((int)id);
                if (itemDto == null)
                {
                    TempData["Error"] = "Item not exists.";
                    return View(itemDto);
                }
                item.ImageUrl = itemDto.ImageUrl;
            }
            item.CategoryId = catId;
            if (item.Image != null)
            {
                var filename = item.Image.FileName;
                var filePathOriginal = Server.MapPath("/Content/Images/");
                string savedFileName = Path.Combine(filePathOriginal, filename);
                try
                {
                    item.Image.SaveAs(savedFileName);
                    Utilities.ResizeImage(Image.FromStream(item.Image.InputStream, true, true), savedFileName);

                    item.ImageUrl = item.Image.FileName;
                }
                catch
                {
                    TempData["Error"] = "Failed to upload image try again";
                    return View(item);
                }
            }
            var itm = MapperManager.GetMapperInstance().Map<ItemDTO>(item);
            var ItemDto = adminManagerBll.AddOrUpdateItem(id, itm);
            if (ItemDto == null)
            {
                TempData["Error"] = id != null ? "Updating the item failed." : "Adding the item failed";
                return View(item);
            }
            TempData["Success"] = id != null ? "Updated the item successfully." : "Successfully added the item";

            ViewBag.Id = ItemDto.Id;
            return View(MapperManager.GetMapperInstance().Map<ItemVM>(ItemDto));
        }

        [HttpGet]
        [Route("Items/Delete/{Id}")]
        public ActionResult DeleteItem(int id)
        {
            var itemDto = adminManagerBll.GetItemById((int)id);
            if (itemDto == null)
            {
                TempData["Error"] = "Item not exists.";
                return RedirectToAction("Details", "Home", new { id = itemDto.Id });
            }
            if (adminManagerBll.DeleteItem(id))
            {
                TempData["Success"] = "Image Deleted successfully.";
                return RedirectToAction("Index", "Home", new { id = itemDto.CategoryId });
            }
            else
            {
                TempData["Error"] = "Failed to remove the image.";
                return RedirectToAction("Details", "Home", new { id = itemDto.Id });
            }
        }

        [HttpGet]
        [Route("Category/Delete/{Id}")]
        public ActionResult DeleteCategory(int id)
        {
            var categoryDto = adminManagerBll.GetCategoryById((int)id);
            if (categoryDto == null)
            {
                TempData["Error"] = "Category not exists.";
                return RedirectToAction("Index", "Home", new { id = id});
            } 
            if (adminManagerBll.DeleteCategory(id))
            {
                TempData["Success"] = "Category Deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to remove the Category.";
            }

            return RedirectToAction("Index", "Home", new { id = id });
        }

        [HttpGet]
        [Route("Items/DeleteImage/{Id}")]
        public ActionResult DeleteImage(int id)
        {
            int itemId = Convert.ToInt32(Request.QueryString["itemId"]);
            var itemDto = adminManagerBll.GetItemById(itemId);
            if (itemDto == null)
            {
                TempData["Error"] = "Item not exists.";
                return RedirectToAction("ItemsAdd","Home", new { Id= itemId });
            }
            if (adminManagerBll.DeleteImage(id))
            {
                TempData["Success"] = "Image Deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to remove the image.";
            }
            return RedirectToAction("ItemsAdd", "Home", new { Id = itemId });
        }

        [HttpGet]
        [Route("Items/PrimaryImage/{Id}")]
        public ActionResult SetImageAsPrimary(int id)
        {
            int itemId = Convert.ToInt32(Request.QueryString["itemId"]);
            var itemDto = adminManagerBll.GetItemById(itemId);
            if (itemDto == null)
            {
                TempData["Error"] = "Item not exists.";
                return RedirectToAction("ItemsAdd", "Home", new { Id = itemId });
            }
            if (adminManagerBll.SetImageAsPrimary(id,itemId))
            {
                TempData["Success"] = "Successfully set the image as primary.";
            }
            else
            {
                TempData["Error"] = "Failed to set the image as primary.";
            }
            return RedirectToAction("ItemsAdd", "Home", new { Id = itemId });
        }
        [HttpPost]
        [Route("Category/{Id?}")]
        public ActionResult CategoryAdd(int? id, CategoryVM category)
        {
            ViewBag.Id = id;
            ViewBag.ButtonName = "Save";
            if (id != null)
            {
                ViewBag.ButtonName = "Update";
            }
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if (id == null && category.Image == null)
            {
                TempData["Error"] = "Select a slider image.";
                return View(category);
            }
            if (category.Image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    category.Image.InputStream.CopyTo(memoryStream);
                }
            }
            if (id != null)
            {
                var categoryDto = adminManagerBll.GetCategoryById((int)id);
                if (categoryDto == null)
                {
                    TempData["Error"] = "Category not exists.";
                    return View(category);
                }
                category.ImageUrl = categoryDto.ImageUrl;
            }
            if (category.Image != null)
            {
                var filename = category.Image.FileName;
                var filePathOriginal = Server.MapPath("/Content/Images/");
                string savedFileName = Path.Combine(filePathOriginal, filename);
                try
                {
                    category.Image.SaveAs(savedFileName);
                    Utilities.ResizeImage(Image.FromStream(category.Image.InputStream, true, true), savedFileName);
                    category.ImageUrl = category.Image.FileName;
                }
                catch(Exception ex) 
                {
                    TempData["Error"] = "Failed to upload image try again";
                    return View(category);
                }
            }
            var cat = MapperManager.GetMapperInstance().Map<CategoryDTO>(category);
            var catDto = adminManagerBll.AddOrUpdateCategory(id, cat);
            if (catDto == null)
            {
                TempData["Error"] = id!=null?"Updating the category failed.":"Adding the category failed";
                return View(category);
            }
            TempData["Success"] = id != null ? "Updated the category successfully." : "Successfully added the category";
            ViewBag.Id = catDto.Id;
            return View(MapperManager.GetMapperInstance().Map<CategoryVM>(catDto));
        }

        [HttpGet]
        [Route("Slider/Delete/{Id}")]
        public ActionResult DeleteSlider(int id)
        {
            var sliderDto = adminManagerBll.GetSliderById((int)id);
            if (sliderDto == null)
            {
                TempData["Error"] = "slider not exists.";
                return RedirectToAction("Index", "Details", new { id = sliderDto.Id });
            }
            if (adminManagerBll.DeleteSlider(id))
            {
                TempData["Success"] = "slider Deleted successfully.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Failed to remove the image.";
                return RedirectToAction("Slider", "Home", new { id = id });
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}