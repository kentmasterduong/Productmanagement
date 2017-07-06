using BL.Product;
using Core.Common;
using DTO.Category;
using DTO.Item;
using DTO.Measure;
using Ivs.Models;
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
        public ActionResult Item(ResearchItemModel model)
        {

            ModelState.Clear();
            if (model == null)
            {
                model = new ResearchItemModel();
                model.Item = new ItemDTO();
                model.Items = new List<ItemDTO>();
            }
            else
            {
                if (model.Item == null)
                {
                    if (Session["model.Item"] != null && model.page > 1)
                    {
                        model.Item = Session["model.Item"] as ItemDTO;
                    }
                    else
                    {
                        model.Item = new ItemDTO();
                    }
                }
                else
                {
                    model.page = 1;
                    Session["model.Item"] = model.Item;
                }
                if (model.Items == null)
                {
                    model.Items = new List<ItemDTO>();
                }


            }


            ItemBL bl = new ItemBL();
            model.page_count = bl.CountData(model.Item);
            List<ItemDTO> list;
            model.Item.page = model.page;
            bl.SearchData(model.Item, out list);
            model.Items = list;
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
                    int count = itemBL.CountData(new ItemDTO() { code = item.code });
                    if (count == 0)
                    {
                        itemBL.InsertData(item);
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
        public ActionResult Update(string id)
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
                dto.id = int.Parse(id);
                bl.SearchData(dto, out list);
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
            return View(LoadItemAddForm(dto));
        }

        [HttpPost]
        public ActionResult UpdateItem(ItemDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.created_by = 123;
                    item.updated_by = 123;
                    ItemBL itemBL = new ItemBL();
                    itemBL.UpdateData(item);

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

            return View("Update", LoadItemAddForm(item));
        }

        [HttpPost]
        public ActionResult DeleteItem(string id)
        {
            try
            {
                ItemBL itemBL = new ItemBL();
                itemBL.DeleteData(id.ParseInt32());

                return RedirectToAction("Item");
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
                return RedirectToAction("SubmissionFailed");
            }
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
        public ActionResult View(string id)
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
                dto.id = int.Parse(id);
                bl.SearchData(dto, out list);
                if (list.Count > 0)
                {
                    dto = list[0];
                }
                else
                {
                    TempData["Success"] = "0 row(s) has found.";
                }
            }
            return View(LoadItemAddForm(dto));
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