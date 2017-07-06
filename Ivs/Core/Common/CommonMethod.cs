using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class CommonMethod
    {
        /// <summary>
        /// Convert DataTable to List<T>/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public static List<T> DataTableToDto<T>(this DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }
        }

        private static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }
        /// <summary>
        /// Convert List<T> to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable DtoToDataTable<T>(this IEnumerable<T> items)
        {
            // Create the result table, and gather all properties of a T
            DataTable table = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add the properties as columns to the datatable
            foreach (var prop in props)
            {
                Type propType = prop.PropertyType;

                // Is it a nullable type? Get the underlying type
                if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    propType = new NullableConverter(propType).UnderlyingType;

                table.Columns.Add(prop.Name, propType);
            }

            // Add the property values per T as rows to the datatable
            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null);

                table.Rows.Add(values);
            }

            return table;
        }

        public static bool IsNumber(this string valStr)
        {
            string pattern = @"[\D]";
            Regex myRegex = new Regex(pattern);
            if (myRegex.IsMatch(valStr))
            {
                return false;
            }
            return true;
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            if (str != null)
            {
                return str.Length > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// <para>Convert string to Int32</para>
        /// <para>Return 0: If string value is null or empty</para>
        /// </summary>
        /// <param name="num"></param>
        /// <returns>0</returns>
        public static int ParseInt32(this string num)
        {
            if (num.IsNumber() && num.IsNotNullOrEmpty())
            {
                return int.Parse(num);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// <para>Convert Object to DateTime</para>
        /// <para></para>
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ParseDateTime(this object date)
        {
            if (date is DateTime)
            {
                return (DateTime)date;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// <para>Convert string to decimal</para>
        /// <para>Return 0: If string value is null or empty</para>
        /// </summary>
        /// <param name="num"></param>
        /// <returns>0</returns>
        public static decimal ParseDecimal(this string num)
        {
            if (num.IsNotNullOrEmpty() && num.Length > 0)
            {
                return decimal.Parse(num);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// <para>Convert string to bool</para>
        /// <para>Return false: If string value is null or empty</para>
        /// </summary>
        /// <param name="num"></param>
        /// <returns>0</returns>
        public static bool ParseBool(this string num)
        {
            if (num.IsNotNullOrEmpty() && num.Length > 0)
            {
                return bool.Parse(num);
            }
            else
            {
                return false;
            }
        }

    }
}
