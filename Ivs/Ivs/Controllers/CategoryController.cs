using BL.Product;
using Core.Common;
using DTO.Category;
using Ivs.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Core.Common.CommonMethod;

namespace Ivs.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryBL categoryBL = new CategoryBL();

        [HttpGet]
        public ActionResult Category(CategoryModel model, int page = 1)
        {
            ModelState.Clear();
            if (model == null)
            {
                model = new CategoryModel();
                model.Category = new CategoryDTO();
                model.Categorys = new StaticPagedList<CategoryDTO>(new List<CategoryDTO>(), 1, 20, 0);
            }
            else
            {
                if (model.Category == null)
                {
                    if (Session["model.Category"] != null && page > 1)
                    {
                        model.Category = Session["model.Category"] as CategoryDTO;
                    }
                    else
                    {
                        model.Category = new CategoryDTO();
                    }
                }
                else
                {
                    Session["model.Category"] = model.Category;
                }
            }
            CategoryBL bl = new CategoryBL();
            model.Category.page = page;
            model.Category.page_count = bl.CountData(model.Category);
            TempData["SearchCount"] = model.Category.page_count + " row(s) has found.";
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;
            List<CategoryDTO> list;
            bl.SearchData(model.Category, out list);
            model.Categorys = new StaticPagedList<CategoryDTO>(list, model.Category.page, 20, model.Category.page_count);

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryDTO category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!category.code.Contains(" "))
                    {
                        int count = categoryBL.CountData(new CategoryDTO() { code_key = category.code });
                        if (count == 0)
                        {
                            category.created_by = 123;
                            category.updated_by = 123;
                            CommonData.ReturnCode returnCode = categoryBL.InsertData(category);
                            if (returnCode == CommonData.ReturnCode.Success)
                            {
                                TempData["Success"] = "Inserted Successfully!";
                            }
                            else
                            {
                                TempData["Error"] = "Insert fail";
                            }
                            Session["model.Category"] = null;
                            return RedirectToAction("Category");
                        }
                        else
                        {
                            ModelState.AddModelError("code", "The Code already is existed!");
                            return RedirectToAction("Add");
                        }
                       
                    }
                    else
                    {
                        ModelState.AddModelError("code", "The code must not have spaces ");
                    }
                }
                else
                {

                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
            }
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;
            return View("Add", category);
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            if (!id.IsNotNullOrEmpty())
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Category");
            }

            CategoryDTO model = new CategoryDTO();
            if (id.IsNumber())
            {
                SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
                ViewBag.ListCategory = listCategory;
                List<CategoryDTO> list;
                categoryBL.SearchData(new CategoryDTO { id = int.Parse(id) }, out list);
                if (list.Count > 0)
                {
                    model = list[0];
                }
                else
                {
                    TempData["Error"] = "Data has already been deleted by other user!";
                    return RedirectToAction("Category");
                }
            }
            else
            {
                TempData["Error"] = "Error exception";
                return RedirectToAction("Category");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateCategory(CategoryDTO category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    category.created_by = 123;
                    category.updated_by = 123;

                    if(category.id == category.parent_id)
                    {
                        ModelState.AddModelError("parent_id", "Parent Category is duplicated with current category");
                    }
                    else
                    {

                        CommonData.ReturnCode returnCode = categoryBL.UpdateData(category);
                        if (returnCode == CommonData.ReturnCode.Success)
                        {
                            TempData["Success"] = "Update Successfully!";
                        }
                        else
                        {
                            TempData["Error"] = "Update fail";
                        }
                        Session["model.Category"] = null;
                        return RedirectToAction("Category");
                    }
                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
                return RedirectToAction("SubmissionFailed", category);
            }

            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;

            return View("Update", category);
        }

        [HttpPost]
        public ActionResult DeleteCategory(string id)
        {
            //List<string> lstMsg = new List<string>();

            if (!string.IsNullOrEmpty(id))
            {
                CommonData.ReturnCode returnCode = categoryBL.DeleteData(int.Parse(id));
                if (CommonData.ReturnCode.Success == returnCode)
                {
                    TempData["Success"] = "Deleted Successfully!";
                    Session["model.Category"] = null;
                    return RedirectToAction("Category");
                }
            }
            TempData["Error"] = "Delete fail";
            return RedirectToAction("Category");
        }
    }
}