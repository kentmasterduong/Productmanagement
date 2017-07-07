using Core.Common;
using DTO.Category;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace DAL.Dao.Product
{
    public class CategoryDAO
    {
        private static string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        public static CommonData.ReturnCode SearchData(CategoryDTO inputDto, out List<CategoryDTO> list)
        {
            list = new List<CategoryDTO>();
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = @"  SELECT c.`id`, c.`parent_id`, cpp.`name` AS parent_name, c.`code`, c.`name`, c.`description` 
                    FROM product_category c 
                    Left JOIN (SELECT cp.`name`,  cp.`id` FROM product_category cp ) cpp on cpp.`id` = c.`parent_id`
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

                    if (inputDto.parent_id != null)
                    {
                        sql += " AND c.`parent_id` = @parent_id ";
                        cmd.Parameters.AddWithValue("@parent_id", inputDto.parent_id);
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
                            foreach(DataRow item in dt.Rows)
                            {
                                CategoryDTO dto = new CategoryDTO();

                                dto.id = int.Parse(item["id"].ToString());
                                dto.parent_name = item["parent_name"].ToString();
                                dto.parent_id = item["parent_id"].ToString().ParseInt32();
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

        public static CommonData.ReturnCode SelectSimpleData(out List<CategoryDropdownlistDTO> list)
        {
            list = new List<CategoryDropdownlistDTO>();

            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success ;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT id, code, name FROM product_category", con))
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
                                CategoryDropdownlistDTO dto = new CategoryDropdownlistDTO();

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
        
        public static CommonData.ReturnCode InsertData(CategoryDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            try
            {

                using (MySqlConnection connect = new MySqlConnection(constr))
                {
                    using (MySqlCommand command = new MySqlCommand(
                        @"INSERT INTO product_category(
                    `code`, `name`, `parent_id`, `description`
                    , `created_datetime`, `created_by`,`updated_datetime`,`updated_by`
                    ) VALUES (
                    @code, @name, @parent_id, @description
                    , SYSDATE(), @created_by, SYSDATE(), @updated_by)", connect))
                    {
                        command.Parameters.AddWithValue("@code", dto.code);
                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@parent_id", dto.parent_id);
                        command.Parameters.AddWithValue("@description", dto.description);
                        command.Parameters.AddWithValue("@created_by", dto.created_by);
                        command.Parameters.AddWithValue("@updated_by", dto.updated_by);



                        connect.Open();
                        command.ExecuteNonQuery();
                    }


                }
            }catch(Exception ex)
            {
                returnCode = CommonData.ReturnCode.UnSuccess;
            }
            return returnCode;
        }

        public static CommonData.ReturnCode UpdateData(CategoryDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;

            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"Update product_category Set
                    `name` = @name, `parent_id` = @parent_id, `description` = @description,
                    `updated_datetime` = SYSDATE(), `updated_by` = @updated_by Where `id` = @ID", connect))
                {
                    try
                    {

                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@parent_id", dto.parent_id);
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

        public static int CountData(CategoryDTO dto)
        {
            int count = 0;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = " SELECT COUNT(*) FROM product_category c WHERE TRUE ";


                    #region Where Clause
                    if (dto.id != null)
                    {
                        sql += " AND c.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", dto.id);
                    }

                    if (dto.code_key.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`code` = @Code ";
                        cmd.Parameters.AddWithValue("@Code", dto.code_key);
                    }

                    if (dto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", dto.code);
                    }

                    if (dto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", dto.name);
                    }

                    if (dto.parent_id != null)
                    {
                        sql += " AND c.`parent_id` = @parent_id ";
                        cmd.Parameters.AddWithValue("@parent_id", dto.parent_id);
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
