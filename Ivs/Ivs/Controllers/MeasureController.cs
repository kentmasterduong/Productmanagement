using BL.Product;
using Core.Common;
using DTO.Measure;
using Ivs.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ivs.Controllers
{
    public class MeasureController : Controller
    {
        MeasureBL measureBL = new MeasureBL();

        // GET: Measure
        [HttpGet]
        public ActionResult Measure(MeasureModel model, int page = 1)
        {
            ModelState.Clear();
            if (model == null)
            {
                model = new MeasureModel();
                model.Measure = new MeasureDTO();
                model.Measures = new StaticPagedList<MeasureDTO>(new List<MeasureDTO>(), 1, 20, 0);
            }
            else
            {
                if (model.Measure == null)
                {
                    if (Session["model.Measure"] != null && page > 1)
                    {
                        model.Measure = Session["model.Measure"] as MeasureDTO;
                    }
                    else
                    {
                        model.Measure = new MeasureDTO();
                    }
                }
                else
                {
                    Session["model.Measure"] = model.Measure;
                }
            }
            model.Measure.page = page;
            model.Measure.page_count = measureBL.CountData(model.Measure);
            TempData["SearchCount"] = model.Measure.page_count + " row(s) has found.";
            List<MeasureDTO> list;
            measureBL.SearchData(model.Measure, out list);
            model.Measures = new StaticPagedList<MeasureDTO>(list, model.Measure.page, 20, model.Measure.page_count);

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMeasure(MeasureDTO measure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!measure.code.Contains(" "))
                    {
                        int count = measureBL.CountData(new MeasureDTO() { code_key = measure.code });
                        if (count == 0)
                        {
                            measure.created_by = 123;
                            measure.updated_by = 123;
                            CommonData.ReturnCode returnCode = measureBL.InsertData(measure);
                            if (returnCode == CommonData.ReturnCode.Success)
                            {
                                TempData["Success"] = "Inserted Successfully!";
                            }
                            else
                            {
                                TempData["Error"] = "Insert fail";
                            }
                            Session["model.Measure"] = null;
                            return RedirectToAction("Measure");
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

            return View("Add", measure);
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            if (!id.IsNotNullOrEmpty())
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Measure");
            }

            MeasureDTO model = new MeasureDTO();
            if (id.IsNumber())
            {
                List<MeasureDTO> list;
                measureBL.SearchData(new MeasureDTO { id = int.Parse(id) }, out list);
                if (list.Count > 0)
                {
                    model = list[0];
                }
                else
                {
                    TempData["Error"] = "Data has already been deleted by other user!";
                    return RedirectToAction("Measure");
                }
            }
            else
            {
                TempData["Error"] = "Error exception";
                return RedirectToAction("Measure");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateMeasure(MeasureDTO measure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    measure.created_by = 123;
                    measure.updated_by = 123;


                    CommonData.ReturnCode returnCode = measureBL.UpdateData(measure);
                    if (returnCode == CommonData.ReturnCode.Success)
                    {
                        TempData["Success"] = "Update Successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Update fail";
                    }
                    Session["model.Measure"] = null;
                    return RedirectToAction("Measure");

                }
            }
            catch (DataException dex)
            {
                ModelState.AddModelError("", "Lỗi không xác định");
                return RedirectToAction("SubmissionFailed", measure);
            }

            return View("Update", measure);
        }

        [HttpPost]
        public ActionResult DeleteMeasure(string id)
        {
            //List<string> lstMsg = new List<string>();

            if (!string.IsNullOrEmpty(id))
            {
                CommonData.ReturnCode returnCode = measureBL.DeleteData(int.Parse(id));
                if (CommonData.ReturnCode.Success == returnCode)
                {
                    TempData["Success"] = "Deleted Successfully!";
                    Session["model.Measure"] = null;
                    return RedirectToAction("Measure");
                }
            }
            TempData["Error"] = "Delete fail";
            return RedirectToAction("Measure");
        }
    }
}