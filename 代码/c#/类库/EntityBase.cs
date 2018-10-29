using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTool
{

    public interface TableInfoInface
    {
        EntityBase GetTableInfo();

    }

    public class EntityBase 
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表说明
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 列相关信息（是否为主键，长度，）
        /// </summary>
        private Dictionary<string, ColumnBase> ColumnValueDic = new Dictionary<string, ColumnBase>();

        /// <summary>
        /// 主键集合
        /// </summary>
        private List<string> keys = new List<string>();

        /// <summary>
        /// 值发生变化的列名集合
        /// </summary>
        private List<string> ChangeColumnNames = new List<string>();

        #region 设置属性值的多个重载相关


      

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyFieldName">属性字段名</param>
        /// <param name="Value">要设置的值</param>
        protected internal void setProperty(string propertyFieldName, object Value, System.Data.DbType dbType, int maxLength = 50,bool isPrimary = false)
        {
            if (dbType==System.Data.DbType.String)
            {
                if (Value != null && maxLength > 0 && ((string)Value).Length > maxLength)
                    throw new Exception("字段" + propertyFieldName + "的实际长度超出了最大长度" + maxLength);
            }
            setPropertyValueAndLength(propertyFieldName,  Value, maxLength, dbType, isPrimary);
        }

        /// <summary>
        /// 设置属性值和长度信息
        /// </summary>
        /// <param name="propertyFieldName">属性字段名称</param>
        /// <param name="propertyIndex">属性字段索引位置</param>
        /// <param name="Value">属性值</param>
        /// <param name="length"></param>
        /// <param name="dbType">字段类型</param>
        private void setPropertyValueAndLength(string propertyFieldName, object value, int length, System.Data.DbType dbType, bool isPrimary)
        {
            bool setValueFlag = false;
            if (string.IsNullOrEmpty(propertyFieldName))
            {
                throw new ArgumentException("属性字段名称不能为空或者Null。");
            }
            if (ColumnValueDic.Keys.Contains(propertyFieldName))
            {
                ColumnValueDic[propertyFieldName] = new ColumnBase() { ColumnName = propertyFieldName, ColumnType = dbType, IsPrimary = isPrimary, ColumnLength = length, Value = value };
                setValueFlag = true;
            }
            else
            {
                ColumnValueDic.Add(propertyFieldName, new ColumnBase() { ColumnName = propertyFieldName, ColumnType = dbType, IsPrimary = isPrimary, ColumnLength = length, Value = value });
                setValueFlag = true;
            }
            if (!setValueFlag)
            {
                if (isPrimary)
                {
                    keys.Add(propertyFieldName);
                }
                ChangeColumnNames.Add(propertyFieldName);
                //要赋值的字段不在实体类定义的字段名中，抛出异常
                throw new ArgumentException("属性字段名称 [" + propertyFieldName + "] 无效，请检查实体类的当前属性定义和重载的SetFieldNames 方法中对PropertyNames 的设置。");
            }
            //原来的代码已经优化

        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="propertyFieldName">属性名称</param>
        /// <returns>属性值</returns>
        protected T getProperty<T>(string propertyFieldName)
        {
            bool getValueFlag = false;
            if (string.IsNullOrEmpty(propertyFieldName))
            {
                throw new ArgumentException("属性字段名称不能为空或者Null。");
            }
            ColumnBase value = null;
            getValueFlag = ColumnValueDic.TryGetValue(propertyFieldName, out value);
            if (!getValueFlag)
            {
                //要赋值的字段不在实体类定义的字段名中，抛出异常
                throw new ArgumentException("属性字段名称 [" + propertyFieldName + "] 无效，请检查实体类的当前属性定义和重载的SetFieldNames 方法中对PropertyNames 的设置。");
            }
            return (T)value.Value;
        }

        #endregion
    }

   
    /// <summary>
    /// 数据库列相关信息
    /// </summary>
    public class ColumnBase
    {
        public bool IsPrimary { get; set; }
        public string ColumnName { get; set; }

        public DbType ColumnType { get; set; }

        public int ColumnLength { get; set; }

        public object Value { get; set; }

        public string Comment { get; set; }
    }
}
