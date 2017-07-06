using Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using DAL.Dao.Product;
using DTO.Item;
using Core.Common;

namespace BL.Product
{
    public class ItemBL : IBL
    {
        public CommonData.ReturnCode DeleteData(int id)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            returnCode = ItemDAO.DeleteData(id);
            return returnCode;
        }

        public CommonData.ReturnCode InsertData(IDTO insertDto)
        {
            CommonData.ReturnCode returnCode = ItemDAO.InsertData(insertDto as ItemDTO);
            return returnCode;
        }

        public CommonData.ReturnCode SearchData(IDTO searchDto, out DataTable dtResult)
        {
            throw new NotImplementedException();
        }

        public CommonData.ReturnCode SearchData(IDTO searchDto, out List<ItemDTO> list)
        {
            CommonData.ReturnCode returnCode = ItemDAO.SearchData(searchDto as ItemDTO, out list);
            
            return returnCode;
        }

        public CommonData.ReturnCode UpdateData(IDTO updateDto)
        {
            CommonData.ReturnCode returnCode = ItemDAO.UpdateData(updateDto as ItemDTO);
            return returnCode;
        }

        public int CountData(ItemDTO updateDto)
        {
            return ItemDAO.CountData(updateDto);
        }
    }
}
