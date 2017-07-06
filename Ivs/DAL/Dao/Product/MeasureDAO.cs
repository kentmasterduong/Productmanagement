using Core.Common;
using DAL.Dao.Product;
using DTO.Category;
using DTO.Measure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dao.Product
{
    public class MeasureDAO
    {
        static string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        public static CommonData.ReturnCode SearchData(MeasureDTO inputDto, out DataTable dt)
        {
            dt = new DataTable();
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM product_measure", con))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {
                            returnCode = CommonData.ReturnCode.UnSuccess;
                        }
                    }
                }
            }

            return returnCode;
        }
        
        public static CommonData.ReturnCode SelectSimpleData(out List<MeasureDropdownlistDTO> list)
        {
            list = new List<MeasureDropdownlistDTO>();

            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT id, code, name FROM product_measure", con))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                MeasureDropdownlistDTO dto = new MeasureDropdownlistDTO();

                                dto.id = int.Parse(item["id"].ToString());
                                dto.code = item["code"].ToString();
                                dto.name = item["name"].ToString();

                                list.Add(dto);
                            }
                        }
                    }
                }
            }

            return returnCode;
        }
    }
}
