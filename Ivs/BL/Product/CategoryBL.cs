using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL.Dao.Product;
using DTO.Category;
using Core.Common;

namespace BL.Product
{
    public class CategoryBL : IBL
    {
        public CommonData.ReturnCode DeleteData(int id)
        {
            CommonData.ReturnCode returnCode = CategoryDAO.DeleteData(id);
            return returnCode;
        }

        public CommonData.ReturnCode InsertData(IDTO insertDto)
        {
            CommonData.ReturnCode returnCode = CategoryDAO.InsertData(insertDto as CategoryDTO);
            return returnCode;
        }

        public CommonData.ReturnCode UpdateData(IDTO updateDto)
        {
            CommonData.ReturnCode returnCode = CategoryDAO.UpdateData(updateDto as CategoryDTO);
            return returnCode;
        }

        public CommonData.ReturnCode SearchData(IDTO searchDto, out List<CategoryDTO> list )
        {
            CommonData.ReturnCode returnCode = CategoryDAO.SearchData(searchDto as CategoryDTO, out list);
            return returnCode;
        }

        public List<CategoryDropdownlistDTO> SelectDropdownData()
        {
            List<CategoryDropdownlistDTO> list;
            CategoryDAO.SelectSimpleData(out list);
            return list;
        }

        public int CountData(CategoryDTO updateDto)
        {
            return CategoryDAO.CountData(updateDto);
        }

        public CommonData.ReturnCode SearchData(IDTO searchDto, out List<IDTO> list)
        {
            throw new NotImplementedException();
        }
    }
}
