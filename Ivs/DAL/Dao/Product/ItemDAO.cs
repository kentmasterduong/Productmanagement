using Core.Common;
using DTO.Category;
using DTO.Item;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Common.CommonMethod;


namespace DAL.Dao.Product
{
    public class ItemDAO
    {
        private static string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        public static CommonData.ReturnCode SearchData(ItemDTO inputDto, out List<ItemDTO> list)
        {
            list = new List<ItemDTO>();
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = string.Empty;

                    sql = @"SELECT i.`id`, i.`code`, i.`name`, c.`name` as category_name, i.`discontinued_datetime`, i.`dangerous` ";
                    if (inputDto.id != null)
                    {
                        sql += @", i.`inventory_measure_id`, i.`inventory_expired`, i.`inventory_standard_cost`, i.`specification`, i.`description`
                         , i.`inventory_list_price`, i.`manufacture_day`, i.`manufacture_make`, i.`manufacture_tool`
                         , i.`manufacture_finished_goods`, i.`manufacture_size`, i.`manufacture_size_measure_id`
                         , i.`manufacture_weight`, i.`manufacture_weight_measure_id`, i.`manufacture_style`, i.`category_id`
                         , i.`manufacture_class`, i.`manufacture_color` ";
                    }

                    sql += @" FROM product_item AS i 
					        left JOIN product_category c on c.`id` = i.`category_id` 
                            WHERE TRUE ";


                    #region Where Clause

                    if (inputDto.id != null)
                    {
                        sql += " AND i.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", inputDto.id);
                    }

                    if (inputDto.code_key.IsNotNullOrEmpty())
                    {
                        sql += " AND i.`code` = @Code ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code_key);
                    }

                    if (inputDto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND i.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code);
                    }

                    if (inputDto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND i.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", inputDto.name);
                    }

                    if (inputDto.category_id != null)
                    {
                        sql += " AND i.`category_id` = @category_id ";
                        cmd.Parameters.AddWithValue("@category_id", inputDto.category_id);
                    }

                    #endregion

                    sql += " ORDER BY i.`updated_datetime` DESC";
                    sql += " LIMIT  @start, 20";

                    cmd.Parameters.AddWithValue("@start", 20 * (inputDto.page - 1));


                    cmd.Connection = con;
                    cmd.CommandText = sql;

                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        DataTable dt = new DataTable();
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                ItemDTO dto = new ItemDTO();

                                dto.id = item["id"].ToString().ParseInt32();
                                dto.code = item["code"].ToString();
                                dto.name = item["name"].ToString();
                                dto.category_name = item["category_name"].ToString();
                                //dto.category_parent_name = item["category_parent_name"].ToString();
                                dto.discontinued_datetime = item["discontinued_datetime"].ParseDateTime();
                                dto.dangerous = item["dangerous"].ToString().ParseBool();

                                if (inputDto.id != null)
                                {
                                    dto.specification = item["specification"].ToString();
                                    dto.description = item["description"].ToString();
                                    dto.category_id = item["category_id"].ToString().ParseInt32();
                                    dto.inventory_list_price = item["inventory_list_price"].ToString().ParseInt32();
                                    dto.inventory_measure_id = item["inventory_measure_id"].ToString().ParseInt32();
                                    dto.inventory_expired = item["inventory_expired"].ToString().ParseInt32();
                                    dto.inventory_standard_cost = item["inventory_standard_cost"].ToString().ParseDecimal();
                                    dto.inventory_list_price = item["inventory_list_price"].ToString().ParseDecimal();
                                    dto.manufacture_day = item["manufacture_day"].ToString().ParseDecimal();
                                    dto.manufacture_make = item["manufacture_make"].ToString().ParseBool();
                                    dto.manufacture_tool = item["manufacture_tool"].ToString().ParseBool();
                                    dto.manufacture_finished_goods = item["manufacture_finished_goods"].ToString().ParseBool();
                                    dto.manufacture_size = item["manufacture_size"].ToString();
                                    dto.manufacture_size_measure_id = item["manufacture_size_measure_id"].ToString().ParseInt32();
                                    dto.manufacture_weight = item["manufacture_weight"].ToString();
                                    dto.manufacture_weight_measure_id = item["manufacture_weight_measure_id"].ToString().ParseInt32();
                                    dto.manufacture_style = item["manufacture_style"].ToString();
                                    dto.manufacture_class = item["manufacture_class"].ToString();
                                    dto.manufacture_color = item["manufacture_color"].ToString();
                                }
                                list.Add(dto);
                            }
                            returnCode = 0;
                        }
                        else
                        {
                            returnCode = CommonData.ReturnCode.UnSuccess;
                        }
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {

                }
            }

            return returnCode;
        }

        public static CommonData.ReturnCode InsertData(ItemDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            try
            {
                using (MySqlConnection connect = new MySqlConnection(constr))
                {
                    using (MySqlCommand command = new MySqlCommand(
                        @"INSERT INTO product_item(
                    `code`, `name`, `specification`, `description`, `category_id`, `discontinued_datetime`
                    , `dangerous`, `inventory_measure_id`, `inventory_expired`, `inventory_standard_cost`
                    , `inventory_list_price`, `manufacture_day`,`manufacture_make`, `manufacture_tool`
                    , `manufacture_finished_goods`, `manufacture_size`, `manufacture_size_measure_id`
                    , `manufacture_weight`, `manufacture_weight_measure_id`, `manufacture_style`
                    , `manufacture_class`, `manufacture_color`
                    , `created_datetime`, `created_by`,`updated_datetime`,`updated_by`
                    ) VALUES (
                    @code, @name, @specification, @description, @category_id, discontinued_datetime
                    , @dangerous, @inventory_measure_id, @inventory_expired, @inventory_standard_cost
                    , @inventory_list_price, @manufacture_day, @manufacture_make, @manufacture_tool
                    , @manufacture_finished_goods, @manufacture_size, @manufacture_size_measure_id
                    , @manufacture_weight, @manufacture_weight_measure_id, @manufacture_style
                    , @manufacture_class, @manufacture_color
                    , SYSDATE(), @created_by, SYSDATE(), @updated_by)", connect))
                    {
                        command.Parameters.AddWithValue("@code", dto.code);
                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@specification", dto.specification);
                        command.Parameters.AddWithValue("@description", dto.description);
                        command.Parameters.AddWithValue("@category_id", dto.category_id);
                        command.Parameters.AddWithValue("@discontinued_datetime", dto.discontinued_datetime);
                        command.Parameters.AddWithValue("@dangerous", dto.dangerous);
                        command.Parameters.AddWithValue("@inventory_measure_id", dto.inventory_measure_id);
                        command.Parameters.AddWithValue("@inventory_expired", dto.inventory_expired);
                        command.Parameters.AddWithValue("@inventory_standard_cost", dto.inventory_standard_cost);
                        command.Parameters.AddWithValue("@inventory_list_price", dto.inventory_list_price);
                        command.Parameters.AddWithValue("@manufacture_day", dto.manufacture_day);
                        command.Parameters.AddWithValue("@manufacture_make", dto.manufacture_make);
                        command.Parameters.AddWithValue("@manufacture_tool", dto.manufacture_tool);
                        command.Parameters.AddWithValue("@manufacture_finished_goods", dto.manufacture_finished_goods);
                        command.Parameters.AddWithValue("@manufacture_size", dto.manufacture_size);
                        command.Parameters.AddWithValue("@manufacture_size_measure_id", dto.manufacture_size_measure_id);
                        command.Parameters.AddWithValue("@manufacture_weight", dto.manufacture_weight);
                        command.Parameters.AddWithValue("@manufacture_weight_measure_id", dto.manufacture_weight_measure_id);
                        command.Parameters.AddWithValue("@manufacture_style", dto.manufacture_style);
                        command.Parameters.AddWithValue("@manufacture_class", dto.manufacture_class);
                        command.Parameters.AddWithValue("@manufacture_color", dto.manufacture_color);
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

        public static CommonData.ReturnCode UpdateData(ItemDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;

            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"Update product_item Set
                    `name` = @name, `specification` = @specification, `description` = @description,
                    `category_id` = @category_id, `discontinued_datetime` = @discontinued_datetime,
                    `dangerous` = @dangerous, `inventory_measure_id` = @inventory_measure_id, 
                    `inventory_expired` = @inventory_expired, `inventory_standard_cost` = @inventory_standard_cost,
                    `inventory_list_price` = @inventory_list_price, `manufacture_day` = @manufacture_day,
                    `manufacture_make` = @manufacture_make, `manufacture_tool` = @manufacture_tool,
                    `manufacture_finished_goods` = @manufacture_finished_goods, 
                    `manufacture_size` = @manufacture_size, `manufacture_size_measure_id` = @manufacture_size_measure_id,
                    `manufacture_weight` = @manufacture_weight, `manufacture_weight_measure_id` = @manufacture_weight_measure_id,
                    `manufacture_style` = @manufacture_style,
                    `manufacture_class` = @manufacture_class, `manufacture_color` = @manufacture_color,
                    `updated_datetime` = SYSDATE(), `updated_by` = @updated_by Where `id` = @ID", connect))
                {
                    try
                    {

                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@specification", dto.specification);
                        command.Parameters.AddWithValue("@description", dto.description);
                        command.Parameters.AddWithValue("@category_id", dto.category_id);
                        command.Parameters.AddWithValue("@discontinued_datetime", dto.discontinued_datetime);
                        command.Parameters.AddWithValue("@dangerous", dto.dangerous);
                        command.Parameters.AddWithValue("@inventory_measure_id", dto.inventory_measure_id);
                        command.Parameters.AddWithValue("@inventory_expired", dto.inventory_expired);
                        command.Parameters.AddWithValue("@inventory_standard_cost", dto.inventory_standard_cost);
                        command.Parameters.AddWithValue("@inventory_list_price", dto.inventory_list_price);
                        command.Parameters.AddWithValue("@manufacture_day", dto.manufacture_day);
                        command.Parameters.AddWithValue("@manufacture_make", dto.manufacture_make);
                        command.Parameters.AddWithValue("@manufacture_tool", dto.manufacture_tool);
                        command.Parameters.AddWithValue("@manufacture_finished_goods", dto.manufacture_finished_goods);
                        command.Parameters.AddWithValue("@manufacture_size", dto.manufacture_size);
                        command.Parameters.AddWithValue("@manufacture_size_measure_id", dto.manufacture_size_measure_id);
                        command.Parameters.AddWithValue("@manufacture_weight", dto.manufacture_weight);
                        command.Parameters.AddWithValue("@manufacture_weight_measure_id", dto.manufacture_weight_measure_id);
                        command.Parameters.AddWithValue("@manufacture_style", dto.manufacture_style);
                        command.Parameters.AddWithValue("@manufacture_class", dto.manufacture_class);
                        command.Parameters.AddWithValue("@manufacture_color", dto.manufacture_color);
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
                    @"Delete FROM product_item Where `id` = @ID", connect))
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

        public static int CountData(ItemDTO dto)
        {
            int count = 0;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = " SELECT COUNT(id) FROM product_item i WHERE TRUE ";

                    #region Where Clause
                    if (dto.id != null)
                    {
                        sql += " AND i.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", dto.id);
                    }

                    if (dto.code_key.IsNotNullOrEmpty())
                    {
                        sql += " AND i.`code` = @Code ";
                        cmd.Parameters.AddWithValue("@Code", dto.code_key);
                    }

                    if (dto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND i.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", dto.code);
                    }

                    if (dto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND i.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", dto.name);
                    }

                    if (dto.category_id != null)
                    {
                        sql += " AND i.`category_id` = @category_id ";
                        cmd.Parameters.AddWithValue("@category_id", dto.category_id);
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
