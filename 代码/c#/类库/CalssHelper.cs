using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        
        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于初始化新实体
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="s">数据源实体</param>
        /// <returns>返回的新实体</returns>
        public static D Mapper<D, S>(S s)
        {
            D d = Activator.CreateInstance<D>(); //构造新实例
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in Typed.GetProperties())
                    {
                        if (dp.Name == sp.Name && dp.PropertyType == sp.PropertyType && dp.Name != "Error" && dp.Name != "Item")//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }


        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于没有新建实体之间
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="d">返回的实体</param>
        /// <param name="s">数据源实体</param>
        /// <returns></returns>
        public static D MapperToModel<D, S>(D d, S s)
        {
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in Typed.GetProperties())
                    {
                        if (dp.Name == sp.Name && dp.PropertyType == sp.PropertyType && dp.Name != "Error" && dp.Name != "Item")//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }
        
        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于没有新建实体之间
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="targetObj">返回的实体</param>
        /// <param name="sourceObj">数据源实体</param>
        /// <returns></returns>
        public static string JsonUpdateObj<T>(string jsonStr, object targetObj)
        {
            string result = string.Empty;   
            try
            {
                JObject sourceTobj = JsonConvert.DeserializeObject<JObject>(jsonStr);
                T sourceObj = JsonConvert.DeserializeObject<T>(jsonStr);
                var sourceTypes = sourceObj.GetType();//获得类型  
                var targetTypes = targetObj.GetType();
                foreach (var item in sourceTobj)
                {
                    foreach (PropertyInfo sourceField in sourceTypes.GetProperties())//获得类型的属性字段  
                    {
                        if (item.Key.Equals(sourceField.Name))
                        {
                            foreach (PropertyInfo targetField in targetTypes.GetProperties())
                            {
                                if (targetField.Name == sourceField.Name && targetField.PropertyType == sourceField.PropertyType)//判断属性名是否相同  
                                {
                                    try
                                    {
                                        targetField.SetValue(targetObj, sourceField.GetValue(sourceObj, null), null);//获得s对象属性的值复制给d对象的属性  
                                    }
                                    catch (Exception ex)
                                    {
                                        result = ex.Message;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result= ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于没有新建实体之间
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="targetObj">返回的实体</param>
        /// <param name="sourceObj">数据源实体</param>
        /// <returns></returns>
        public static string  MapperToModel2(Object sourceObj,object targetObj)
        {
            try
            {
                var sourceTypes = sourceObj.GetType();//获得类型  
                var targetTypes = targetObj.GetType();
                foreach (PropertyInfo sourceField in sourceTypes.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo targetField in targetTypes.GetProperties())
                    {
                        if (targetField.Name == sourceField.Name && targetField.PropertyType == sourceField.PropertyType)//判断属性名是否相同  
                        {
                            try
                            {
                                targetField.SetValue(targetObj, sourceField.GetValue(sourceObj, null), null);//获得s对象属性的值复制给d对象的属性  
                            }
                            catch (Exception)
                            {

                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

    }
}
