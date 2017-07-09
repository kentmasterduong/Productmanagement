using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL.Dao.Product;
using DTO.Category;
using DTO.Measure;
using Core.Common;

namespace BL.Product
{
    public class MeasureBL : IBL
    {
        public CommonData.ReturnCode DeleteData(int id)
        {
            CommonData.ReturnCode returnCode = MeasureDAO.DeleteData(id);
            return returnCode;
        }

        public CommonData.ReturnCode InsertData(IDTO insertDto)
        {
            CommonData.ReturnCode returnCode = MeasureDAO.InsertData(insertDto as MeasureDTO);
            return returnCode;
        }
        public CommonData.ReturnCode SearchData(IDTO searchDto, out List<MeasureDTO> list)
        {
            CommonData.ReturnCode returnCode = MeasureDAO.SearchData(searchDto as MeasureDTO, out list);
            return returnCode;
        }

        public CommonData.ReturnCode UpdateData(IDTO updateDto)
        {
            CommonData.ReturnCode returnCode = MeasureDAO.UpdateData(updateDto as MeasureDTO);
            return returnCode;
        }

        public List<MeasureDropdownlistDTO> SelectDropdownData()
        {
            List<MeasureDropdownlistDTO> list;
            MeasureDAO.SelectSimpleData(out list);
            return list;
        }

        public CommonData.ReturnCode SearchData(IDTO searchDto, out List<IDTO> dtResult)
        {
            throw new NotImplementedException();
        }

        public int CountData(IDTO searchDto)
        {
            return MeasureDAO.CountData(searchDto as MeasureDTO);
        }
    }
}
