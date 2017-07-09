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
                    SELECT m.`id`, m.`code`, m.`name`, m.`description` 
                    FROM product_measure m
                    WHERE TRUE  ";


                    #region Where Clause
                    if (inputDto.id != null)
                    {
                        sql += " AND m.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", inputDto.id);
                    }

                    if (inputDto.code_key.IsNotNullOrEmpty())
                    {
                        sql += " AND m.`code` = @Code ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code_key);
                    }

                    if (inputDto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND m.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code);
                    }

                    if (inputDto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND m.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", inputDto.name);
                    }
                    #endregion

                    sql += " ORDER BY m.`updated_datetime` DESC";
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

        public static CommonData.ReturnCode InsertData(MeasureDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            try
            {

                using (MySqlConnection connect = new MySqlConnection(constr))
                {
                    using (MySqlCommand command = new MySqlCommand(
                        @"INSERT INTO product_measure(
                    `code`, `name`, `description`
                    , `created_datetime`, `created_by`,`updated_datetime`,`updated_by`
                    ) VALUES (
                    @code, @name, @description
                    , SYSDATE(), @created_by, SYSDATE(), @updated_by)", connect))
                    {
                        command.Parameters.AddWithValue("@code", dto.code);
                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@description", dto.description);
                        command.Parameters.AddWithValue("@created_by", dto.created_by);
                        command.Parameters.AddWithValue("@updated_by", dto.updated_by);



                        connect.Open();
                        command.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception ex)
            {
                returnCode = CommonData.ReturnCode.UnSuccess;
            }
            return returnCode;
        }

        public static CommonData.ReturnCode UpdateData(MeasureDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;

            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"Update product_measure Set
                    `name` = @name, `description` = @description,
                    `updated_datetime` = SYSDATE(), `updated_by` = @updated_by Where `id` = @ID", connect))
                {
                    try
                    {

                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@description", dto.description);
                        command.Parameters.AddWithValue("@updated_by", dto.updated_by);
                        command.Parameters.AddWithValue("@ID", dto.id);



                        connect.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        returnCode = CommonData.ReturnCode.UnSuccess;
                    }


                }


            }
            return returnCode;
        }

        public static CommonData.ReturnCode DeleteData(int id)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;

            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"Delete FROM product_category Where `id` = @ID; 
                    Update product_category set parent_id = 0 Where `id` = @ID;
                    Update product_item set category_id = 0 Where category_id = @ID;", connect))
                {
                    try
                    {
                        connect.Open();
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        returnCode = CommonData.ReturnCode.UnSuccess;
                    }
                }


            }
            return returnCode;
        }

        public static int CountData(MeasureDTO dto)
        {
            int count = 0;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = " SELECT COUNT(*) FROM product_measure m WHERE TRUE ";


                    #region Where Clause
                    if (dto.id != null)
                    {
                        sql += " AND m.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", dto.id);
                    }

                    if (dto.code_key.IsNotNullOrEmpty())
                    {
                        sql += " AND m.`code` = @Code ";
                        cmd.Parameters.AddWithValue("@Code", dto.code_key);
                    }

                    if (dto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND m.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", dto.code);
                    }

                    if (dto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND m.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", dto.name);
                    }
                    #endregion

                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    count = int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            return count;
        }
    }
}
