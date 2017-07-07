using BL.Product;
using Core.Common;
using DTO.Category;
using DTO.Item;
using DTO.Measure;
using Ivs.Models;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Ivs.Controllers
{
    public class ItemController : Controller
    {
        CategoryBL categoryBL = new CategoryBL();

        [HttpGet]
        public ActionResult Item(ItemModel model, int page = 1)
        {

            ModelState.Clear();
            if (model == null)
            {
                model = new ItemModel();
                model.Item = new ItemSearchDTO();
                model.Items = new StaticPagedList<ItemDTO>(new List<ItemDTO>(), 1, 20, 0);
            }
            else
            {
                if (model.Item == null)
                {
                    if (Session["model.Item"] != null && page > 1)
                    {
                        model.Item = Session["model.Item"] as ItemSearchDTO;
                    }
                    else
                    {
                        model.Item = new ItemSearchDTO();
                    }
                }
                else
                {
                    Session["model.Item"] = model.Item;
                }
            }
            ItemBL bl = new ItemBL();
            model.Item.page_count = bl.CountData(model.Item);
            TempData["SearchCount"] = model.Item.page_count + " row(s) has found.";
            List<ItemDTO> list;
            model.Item.page = page;
            bl.SearchData(model.Item, out list);
            model.Items = new StaticPagedList<ItemDTO>(list, model.Item.page, 20, model.Item.page_count);
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Add", LoadItemAddForm(new ItemDTO()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(ItemDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.created_by = 123;
                    ItemBL itemBL = new ItemBL();
                    int count = itemBL.CountData(new ItemSearchDTO() { code = item.code });
                    if (count == 0)
                    {
                        itemBL.InsertData(item);
                        Session["model.Item"] = null;
                    }
                    else
                    {
                        TempData["Error"] = "The Code already is exister!";
                        return RedirectToAction("Add");
                    }
                    return RedirectToAction("Item");
                }
                else
                {
                    CheckRegex();
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
                return RedirectToAction("SubmissionFailed", item);
            }

            return View("Add", LoadItemAddForm(item));
        }

        [HttpGet]
        public ActionResult Update(string id, string layout)
        {
            ItemDTO dto = new ItemDTO();

            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Item");
            }

            if (id.IsNumber())
            {
                List<ItemDTO> list;
                ItemBL bl = new ItemBL();

                bl.SearchData(new ItemSearchDTO { id = int.Parse(id) }, out list);
                if (list.Count > 0)
                {
                    dto = list[0];
                }
                else
                {
                    TempData["Success"] = "0 row(s) has found.";
                    return RedirectToAction("Item");
                }
            }
            if (!layout.IsNotNullOrEmpty())
            {
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return View(LoadItemAddForm(dto));
        }

        [HttpPost]
        public ActionResult UpdateItem(ItemDTO item, string layout)
        {
            if (ModelState.IsValid)
            {
                item.created_by = 123;
                item.updated_by = 123;
                ItemBL itemBL = new ItemBL();
                itemBL.UpdateData(item);

                TempData["Success"] = "Update Successful";
                Session["model.Item"] = null;
                return RedirectToAction("Item");
            }
            else
            {
                CheckRegex();
            }

            if (!layout.IsNotNullOrEmpty())
            {
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            }
            return View("Update", LoadItemAddForm(item));
        }

        [HttpPost]
        public ActionResult DeleteItem(string id)
        {
            ItemBL itemBL = new ItemBL();
            itemBL.DeleteData(id.ParseInt32());
            TempData["Success"] = "Delete Successful";
            Session["model.Item"] = null;

            return RedirectToAction("Item");
        }

        private ItemDTO LoadItemAddForm(ItemDTO item)
        {
            if (item == null)
            {
                item = new ItemDTO();
            }
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;


            MeasureBL measureBL = new MeasureBL();
            SelectList listMeasure = new SelectList(measureBL.SelectDropdownData(), "id", "name");
            ViewBag.listMeasure = listMeasure;


            return item;
        }

        [HttpGet]
        public ActionResult ViewItem(string id)
        {
            ItemDTO dto = new ItemDTO();

            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Category");
            }

            if (id.IsNumber())
            {
                List<ItemDTO> list;
                ItemBL bl = new ItemBL();

                bl.SearchData(new ItemSearchDTO { id = int.Parse(id) }, out list);
                if (list.Count > 0)
                {
                    dto = list[0];
                }
                else
                {
                    TempData["Success"] = "0 row(s) has found.";
                }
            }
            return View("View", LoadItemAddForm(dto));
        }

        private void CheckRegex()
        {
            if (this.ModelState["inventory_expired"].Errors.Count == 1 && this.ModelState["inventory_expired"].Errors[0].ErrorMessage.Contains("is not valid for"))
            {
                this.ModelState["inventory_expired"].Errors.Clear();
                this.ModelState["inventory_expired"].Errors.Add("Inventory Expired must be numeric");
            }
            if (this.ModelState["inventory_standard_cost"].Errors.Count == 1 && this.ModelState["inventory_standard_cost"].Errors[0].ErrorMessage.Contains("is not valid for"))
            {
                this.ModelState["inventory_standard_cost"].Errors.Clear();
                this.ModelState["inventory_standard_cost"].Errors.Add("Inventory Standard Cost must be numeric");
            }
            if (this.ModelState["inventory_list_price"].Errors.Count == 1 && this.ModelState["inventory_list_price"].Errors[0].ErrorMessage.Contains("is not valid for"))
            {
                this.ModelState["inventory_list_price"].Errors.Clear();
                this.ModelState["inventory_list_price"].Errors.Add("Inventory List Price");
            }
            if (this.ModelState["manufacture_day"].Errors.Count == 1 && this.ModelState["manufacture_day"].Errors[0].ErrorMessage.Contains("is not valid for"))
            {
                this.ModelState["manufacture_day"].Errors.Clear();
                this.ModelState["manufacture_day"].Errors.Add("Manufacture Day must be numeric");
            }
        }
    }
}