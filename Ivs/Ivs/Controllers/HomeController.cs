using BL.Product;
using DTO.Category;
using DTO.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ivs.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Category();

            //Measure();

            // Item();

            //UpdateItem();

            return View();
        }

        void Item()
        {
            List<CategoryDropdownlistDTO> list = new CategoryBL().SelectDropdownData();
            int cateIndex = list.Count - 1;
            DateTime date = DateTime.Now;
            for (int i = 0; i < 100000; i++)
            {
                if (cateIndex == 0)
                    cateIndex = list.Count - 1;
                ItemDTO dto = new ItemDTO();
                dto.code = "Mã_" + i;
                dto.name = "Tên_" + i;

                dto.category_id = list[cateIndex--].id;

                dto.inventory_measure_id = 0;

                dto.manufacture_size_measure_id = 0;

                dto.manufacture_weight_measure_id = 0;
                dto.dangerous = i % 2 == 0;
                dto.specification = "Specification_" + i;
                dto.description = "Mô tả_" + i;
                date = date.AddDays(1);
                dto.discontinued_datetime = date;
                dto.inventory_expired = i;
                dto.inventory_standard_cost = i;
                dto.inventory_list_price = i;
                dto.manufacture_day = i;
                dto.manufacture_make = i % 2 != 0;
                dto.manufacture_tool = i % 2 != 0;
                dto.manufacture_finished_goods = i % 2 == 0;
                dto.manufacture_size = "Size_" + i;
                dto.manufacture_weight = "Weight_" + i;
                dto.manufacture_style = "Style_" + i;
                dto.manufacture_class = "Class_" + i;
                dto.manufacture_color = "Color_" + i;

                dto.created_by = 123;
                dto.updated_by = 123;

                new ItemBL().InsertData(dto);



            }
        }

        void UpdateItem()
        {

            int count = new ItemBL().CountData(new ItemSearchDTO());

            DateTime date = DateTime.Now;
            for (int i = 0; i < count / 20; i++)
            {
                List<ItemDTO> list = new List<ItemDTO>();
                new ItemBL().SearchData(new ItemDTO() { page = i }, out list);
                foreach (var item in list)
                {
                    date = date.AddDays(1);
                    item.discontinued_datetime = date;
                    new ItemBL().UpdateData(item);
                }
            }

        }

        void Measure()
        {

            //            int indexCategory = 0;
            //            int indexMeasure = 0;
            //            List<int> categoryList = new List<int>()
            //            {
            //                25,
            //30,
            //31,
            //32,
            //46,
            //47,
            //48,
            //63,
            //64,
            //66,
            //67,
            //70,
            //71,
            //            };
            //            List<int> measureList = new List<int>() {
            //                7,
            //8,
            //9,
            //10,
            //11,
            //12,
            //13,
            //14,
            //17,
            //18,
            //19,
            //20,
            //21,
            //22,
            //23,
            //24,
            //35,
            //36,
            //37,
            //38,
            //39,
            //40,
            //41,
            //42,
            //43,
            //44,
            //45,
            //46,
            //47,
            //48,
            //49,
            //50,
            //51,
            //52,
            //53,
            //54,
            //55,
            //56,
            //57,
            //58,
            //59,
            //60,
            //61,
            //62,
            //63,
            //64,
            //65,
            //66,
            //67,
            //68,
            //69,
            //70,
            //71,
            //72,
            //73,
            //74,
            //75,
            //76,
            //77,
            //78,
            //79,
            //80,
            //81,
            //82,
            //83,
            //84,
            //85,
            //86,
            //87,
            //88,
            //89,
            //90,
            //91,
            //92,
            //93,
            //94,
            //95,
            //96,
            //97,
            //98,
            //99,
            //100,
            //101,
            //103,
            //104,
            //105,
            //106,
            //108,
            //109,
            //110,
            //111,
            //112,
            //113,
            //114,
            //115,
            //116,
            //117,
            //118,
            //119,
            //120,
            //121,
            //131,
            //132,
            //133,
            //134,};
            //            DateTime date = DateTime.Now;
            //            for (int i = 0; i < 10000; i++)
            //            {
            //                ItemDTO dto = new ItemDTO();
            //                dto.code = "Item_Code_" + i;
            //                dto.name = "Item_Name_" + i;
            //                if (indexCategory >= categoryList.Count)
            //                {
            //                    indexCategory = 0;
            //                }
            //                dto.category_id = categoryList[indexCategory++];

            //                if (indexMeasure >= measureList.Count)
            //                {
            //                    indexMeasure = 0;
            //                }
            //                dto.inventory_measure_id = measureList[indexMeasure++];

            //                if (indexMeasure >= measureList.Count)
            //                {
            //                    indexMeasure = 0;
            //                }
            //                dto.manufacture_size_measure_id = measureList[indexMeasure++];

            //                if (indexMeasure >= measureList.Count)
            //                {
            //                    indexMeasure = 0;
            //                }
            //                dto.manufacture_weight_measure_id = measureList[indexMeasure++];
            //                dto.dangerous = 0;
            //                dto.specification = "Specification_" + i;
            //                dto.description = "Description_" + i;
            //                dto.discontinued_datetime = date.AddDays(1);
            //                dto.inventory_expired = i;
            //                dto.inventory_standard_cost = i;
            //                dto.inventory_list_price = i;
            //                dto.manufacture_day = i;
            //                dto.manufacture_make = 0;
            //                dto.manufacture_tool = 1;
            //                dto.manufacture_finished_goods = 0;
            //                dto.manufacture_size = "Size_" + i;
            //                dto.manufacture_weight = "Weight_" + i;
            //                dto.manufacture_style = "Style_" + i;
            //                dto.manufacture_class = "Class_" + i;
            //                dto.manufacture_color = "Color_" + i;

            //                dto.created_by = 123;

            //                new ItemBL().InsertData(dto);
            //            }
        }

        void Category()
        {

            int indexCategory = 0;

            for (int i = 0; i < 5000; i++)
            {
                CategoryDTO dto = new CategoryDTO();
                dto.code = "Code_" + i;
                dto.name = "Name_" + i;
                dto.parent_id = indexCategory;
                if (i % 20 == 0)
                {
                    indexCategory++;
                }
                dto.description = "Description_" + i;

                dto.created_by = 123;
                dto.updated_by = 123;

                new CategoryBL().InsertData(dto);
            }
        }
    }
}