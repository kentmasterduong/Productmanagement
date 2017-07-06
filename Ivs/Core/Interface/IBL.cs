using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IBL
    {

        CommonData.ReturnCode SearchData(IDTO searchDto, out System.Data.DataTable dtResult);

        CommonData.ReturnCode UpdateData(IDTO updateDto);

        CommonData.ReturnCode InsertData(IDTO insertDto);

        CommonData.ReturnCode DeleteData(int id);

    }
}
