using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;


namespace QTranslateService.Common
{
    public static class ClassHelper
    {

        public delegate object[] CreateRowDelegate<T>(T t);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic GetObjAttribute<T>(Type type, string fieldName)
        {
            Attribute attribute = null;
            //获取对象所属类的属性集合
            IEnumerable<PropertyInfo> properties = type.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if (item.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                {
                    if (item.IsDefined(typeof(T), false))
                    {
                        //字段属性
                        attribute = (Attribute)(item.GetCustomAttributes(false)[0]);
                    }
                }
            }
            if (attribute != null)
            {
                return (T)Activator.CreateInstance(attribute.GetType());
            }
            else
            {
                return "无法找到";
            }

        }


        /// <summary>
        /// 根据类型获得对应的空表结构
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyColumnNames"></param>
        /// <returns></returns>
        public static DataTable ForType(Type type, List<string> keyColumnNames,string tableName="")
        {
            DataTable table = null;
            Type temptype = null;
            DataColumn column = null;
            List<DataColumn> keyDataColumnList = new List<DataColumn>();
            if (type != null)
            {
                //获取对象所属类的属性集合
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
                //创建空的DataTable对象
                if (string.IsNullOrEmpty(tableName))
                {
                    table = new DataTable(type.Name);
                }
                else
                {
                    table = new DataTable(tableName);
                }
                //新创建DataTable根据对象所属类创建列，并指定列数据类型
                foreach (PropertyDescriptor prop in properties)
                {
                    //对DataSet 不支持 System.Nullable<>进行处理
                    temptype = prop.PropertyType; if ((temptype.IsGenericType) && (temptype.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        temptype = temptype.GetGenericArguments()[0];
                    }
                    column = new DataColumn(prop.Name, temptype);
                    table.Columns.Add(column);
                    if (keyColumnNames != null && keyColumnNames.Contains(prop.Name))
                    {
                        keyDataColumnList.Add(column);
                    }
                }
                if (keyDataColumnList.Count > 0)
                {
                    table.PrimaryKey = keyDataColumnList.ToArray();
                }
            }
            return table;
        }


        /// <summary>
        ///如： List<T> list = DAL.ToList();
        ///    dataGridView1.DataSource = list.ToDataTable(a => new object[] { list});
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist, CreateRowDelegate<T> fn)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            // Could add a check to verify that there is an element 0
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    dtReturn.TableName = ((Type)rec.GetType()).Name;
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        //处理Null类型
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return (dtReturn);
        }

    }
}
