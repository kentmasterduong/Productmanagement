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

        public static CommonData.ReturnCode SearchData(MeasureDTO inputDto, out List<MeasureDTO> list)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            list = new List<MeasureDTO>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = @"  
                    SELECT c.`id`, c.`code`, c.`name`, c.`description` 
                    FROM product_measure m
                    WHERE TRUE  ";


                    #region Where Clause
                    if (inputDto.id != null)
                    {
                        sql += " AND c.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", inputDto.id);
                    }

                    if (inputDto.code_key.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`code` = @Code ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code_key);
                    }

                    if (inputDto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code);
                    }

                    if (inputDto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", inputDto.name);
                    }
                    #endregion

                    sql += " ORDER BY c.`updated_datetime` DESC";
                    sql += " LIMIT  @start, 20";

                    if (inputDto.page > 1)
                    {
                        cmd.Parameters.AddWithValue("@start", 20 * (inputDto.page - 1));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@start", 0);
                    }

                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        con.Open();
                        sda.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                MeasureDTO dto = new MeasureDTO();

                                dto.id = int.Parse(item["id"].ToString());
                                dto.code = item["code"].ToString();
                                dto.name = item["name"].ToString();
                                dto.description = item["description"].ToString();

                                list.Add(dto);
                            }
                        }
                        else
                        {

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
