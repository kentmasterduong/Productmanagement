using BL.Product;
using Core.Common;
using DTO.Category;
using Ivs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ivs.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryBL categoryBL = new CategoryBL();
        
        [HttpGet]
        public ActionResult Category(string page, ResearchCategoryModel model)
        {
            ModelState.Clear();
            if (model == null)
            {
                model = new ResearchCategoryModel();
                model.Category = new CategoryDTO();
                model.Categorys = new List<CategoryDTO>();
            }
            else
            {
                if (model.Category == null)
                {
                    model.Category = new CategoryDTO();
                }
                if (model.Categorys == null)
                {
                    model.Categorys = new List<CategoryDTO>();
                }
            }
            CategoryBL bl = new CategoryBL();
            model.page_count = bl.CountData(model.Category);
            SelectList listCategory = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
            ViewBag.ListCategory = listCategory;
            List<CategoryDTO> list;
            bl.SearchData(model.Category, out list);
            model.Categorys = list;

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
            if (!ModelState.IsValid)
            {
                ViewBag.Parent = new SelectList(new List<CategoryDTO>(), "id", "name");
                return View(category);
            }
            CommonData.ReturnCode returnCode = categoryBL.InsertData(category);
            if (((int)CommonData.ReturnCode.Success != returnCode))
            {
                ViewBag.Parent = new SelectList(categoryBL.SelectDropdownData(), "id", "name");
                return View(category);
            }
            TempData["Success"] = "Inserted Successfully!";
            return RedirectToAction("Category");
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            if (string.IsNullOrEmpty(id))
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
                model = list[0];
            }
                
            //int returnCode = _categoryBL.GetByID(long.Parse(id), out Model);
            //if (Model == null)
            //{
            //    TempData["Error"] = "Data has already been deleted by other user!";
            //    return RedirectToAction("Index");
            //}
            //if (!((int)Common.ReturnCode.Succeed == returnCode))
            //{
            //    Model = new CategoryModel();
            //}
            //ViewBag.Parent = new SelectList(_categoryBL.GetListParent(), "id", "name");
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
                    categoryBL.UpdateData(category);

                    return RedirectToAction("Category");
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

            //int returnCode = _categoryBL.Delete(id, out lstMsg);
            //if (!((int)Common.ReturnCode.Succeed == returnCode))
            //{
            //    TempData["Success"] = "Deleted Successfully!";
            //}
            //else
            //{
            //    TempData["Error"] = lstMsg;
            //}
            return RedirectToAction("Category");
        }
    }
}