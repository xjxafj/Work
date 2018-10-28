/*********************
功能描述：链接数据库的基础操作
创建者：fuqiong
创建日期：2017-03-15
 ********************/

using QTranslateService.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace QTranslateService
{
    public class SQLHelper
    {
        //static string constr;   //数据库连接字符串

        //public SQLHelper()
        //{
        //    constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //}
        public static readonly string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        #region private方法
        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句等)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from table</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
                // foreach (SqlParameter parm in cmdParms)
                //    cmd.Parameters.Add(parm);
            }

        }
        #endregion

        #region ExecteNonQuery方法
        /// <summary>
        ///执行一个不需要返回值的SqlCommand命令
        /// </summary>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句等)</param>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>此SqlCommand命令执行后影响的行数</returns>
        private static int ExecteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();                
                //清空SqlCommand中的参数列表
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        ///SqlCommand命令执行后影响的行数(存储过程专用)
        /// </summary>
        /// <param name="spName">存储过程的名字</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>SqlCommand命令执行后影响的行数</returns>
        public static int ExecteNonQueryForProcedure(string spName, params SqlParameter[] commandParameters)
        {
            return ExecteNonQuery(CommandType.StoredProcedure, spName, commandParameters);
        }

        /// <summary>
        ///SqlCommand命令执行后影响的行数(Sql语句专用)
        /// </summary>
        /// <param name="cmdText">T_Sql语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>SqlCommand命令执行后影响的行数</returns>
        public static int ExecteNonQueryForSqlText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecteNonQuery(CommandType.Text, cmdText, commandParameters);
        }

        #endregion

        #region GetDataTable方法
        /// <summary>
        /// 获取执行命令后返回的DataTable
        /// </summary>
        /// <param name="cmdTye">SqlCommand命令类型</param>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个表(DataTable)表示查询得到的数据集</returns>
        private static DataTable ExecuteDataTable(CommandType cmdTye, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                PrepareCommand(cmd, conn, null, cmdTye, cmdText, commandParameters);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
                cmd.Parameters.Clear();
            }
            return dt;
        }


        /// <summary>
        /// 获取执行命令后返回的DataTable（存储过程专用）
        /// </summary>
        /// <param name="spName">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTableForProcedure(string spName, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(CommandType.StoredProcedure, spName, commandParameters);
        }

        /// <summary>
        /// 获取执行命令后返回的DataTable（Sql语句专用）
        /// </summary>
        /// <param name="cmdText"> T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTableForSqlText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(CommandType.Text, cmdText, commandParameters);
        }
        #endregion

        #region ExecuteScalar方法
        /// <summary>
        ///  获取第一行的第一列
        /// </summary>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句等)</param>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns></returns>
        private static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(constr))
            {
                PrepareCommand(cmd, con, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        /// 获取第一行的第一列（存储过程专用）
        /// </summary>
        /// <param name="spName">存储过程的名字</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns></returns>
        public static object ExecuteScalarForProcedure(string spName, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(CommandType.StoredProcedure, spName, commandParameters);
        }

        /// <summary>
        /// 获取第一行的第一列（T_Sql语句专用)
        /// </summary>
        /// <param name="cmdText">者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns></returns>
        public static object ExecuteScalarForSqlText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(CommandType.Text, cmdText, commandParameters);
        }
        #endregion

        #region ExecuteTransaction 方法
        /// <summary>
        /// ExecuteTransaction执行一组SQL语句
        /// </summary>
        /// <param name="sqlAndPara">SQL语句和参数的键值对集合</param>
        /// <param name="earlyTermination">事务中有数据不满足要求是否提前终止事务</param>
        /// <returns></returns>
        public static bool ExecuteTransaction(Dictionary<string, List<SqlParameter[]>> sqlAndPara, bool earlyTermination)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        if (sqlAndPara != null)
                        {
                            bool mark = true;//标记值，记录是否有操作失败的
                            foreach (KeyValuePair<string, List<SqlParameter[]>> kvplist in sqlAndPara)
                            {
                                string cmdText = kvplist.Key;//取SQL语句
                                SqlParameter[] commandParameters = null;
                                if (kvplist.Value.Count > 0)
                                {
                                    foreach (var kvp in kvplist.Value)
                                    {
                                        if (kvp != null)//添加参数
                                            commandParameters = kvp;
                                        PrepareCommand(cmd, conn, transaction, CommandType.Text, cmdText, commandParameters);
                                        int val = cmd.ExecuteNonQuery();
                                        //清空SqlCommand中的参数列表
                                        cmd.Parameters.Clear();
                                        if (earlyTermination)
                                        {
                                            if (val <= 0)
                                            {
                                                mark = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!mark)//如果有某一条执行失败，就回滚
                            {
                                transaction.Rollback(); //事务回滚
                                return false;
                            }
                            else
                            {
                                transaction.Commit();   //事务提交
                                return true;
                            }
                        }
                        else
                            return false;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback(); //事务回滚
                        return false;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }
        }

        #region 注释代码
        //#region ExecuteTransaction 方法
        ///// <summary>
        ///// ExecuteTransaction执行一组SQL语句
        ///// </summary>
        ///// <param name="sqlAndPara">SQL语句和参数的键值对集合</param>
        ///// <param name="earlyTermination">事务中有数据不满足要求是否提前终止事务</param>
        ///// <returns></returns>
        //public static bool ExecuteTransaction(Dictionary<string, SqlParameter[]> sqlAndPara, bool earlyTermination)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    using (SqlConnection conn = new SqlConnection(constr))
        //    {
        //        conn.Open();
        //        using (SqlTransaction transaction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (sqlAndPara != null)
        //                {
        //                    bool mark = true;//标记值，记录是否有操作失败的
        //                    foreach (KeyValuePair<string, SqlParameter[]> kvp in sqlAndPara)
        //                    {
        //                        string cmdText = kvp.Key;//取SQL语句
        //                        SqlParameter[] commandParameters = null;
        //                        if (kvp.Value != null)//添加参数
        //                            commandParameters = kvp.Value;
        //                        PrepareCommand(cmd, conn, transaction, CommandType.Text, cmdText, commandParameters);
        //                        int val = cmd.ExecuteNonQuery();
        //                        //清空SqlCommand中的参数列表
        //                        cmd.Parameters.Clear();
        //                        if (earlyTermination)
        //                        {
        //                            if (val <= 0)
        //                            {
        //                                mark = false;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (!mark)//如果有某一条执行失败，就回滚
        //                    {
        //                        transaction.Rollback(); //事务回滚
        //                        return false;
        //                    }
        //                    else
        //                    {
        //                        transaction.Commit();   //事务提交
        //                        return true;
        //                    }
        //                }
        //                else
        //                    return false;
        //            }
        //            catch
        //            {
        //                transaction.Rollback(); //事务回滚
        //                return false;
        //            }
        //        }
        //    }
        //}

        // #endregion
        #endregion

        #endregion

        #region 批处理
        public static bool ExecuteInsertDocTransaction(string docinfosql, SqlParameter[] paras, string indexessql)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;

                        cmd.CommandText = docinfosql;
                        cmd.Parameters.AddRange(paras);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = indexessql;
                        cmd.Parameters.Clear();
                        cmd.ExecuteNonQuery();

                        transaction.Commit();   //事务提交
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); //事务回滚
                        return false;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 将对象集合批量插入
        /// 20180810 fj
        /// 列名需区分大小写
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">对象集合</param>
        public static int BatchInsert<T>(List<T> list)
        {
            //DataTable dt = ConvertToDataTable(list);
            DataTable dt =list.ToDataTable(a => new object[] { list });
            using (System.Data.SqlClient.SqlBulkCopy bulkcopy = new System.Data.SqlClient.SqlBulkCopy(constr))
            {
                //1 指定数据插入目标表名称  
                bulkcopy.DestinationTableName = dt.TableName;
                bulkcopy.BulkCopyTimeout = 660;
                //2 告诉SqlBulkCopy对象 内存表中的 OrderNO1和Userid1插入到OrderInfos表中的哪些列中
                foreach (DataColumn dc in dt.Columns)
                {
                    bulkcopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }
                //3 将内存表dt中的数据一次性批量插入到OrderInfos表中  
                bulkcopy.WriteToServer(dt);
                
                bulkcopy.Close();
            }
            return list.Count;
        }


        /// <summary>
        /// 将对象集合批量插入
        /// 20180810 fj
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">对象集合</param>
        /// <param name="TableName">表名</param>
        public static int BatchInsert<T>(List<T> list, string TableName)
        {
            DataTable dt = new DataTable(TableName);
            dt = ConvertToDataTable(list);
            dt= list.ToDataTable(a => new object[] { list });
            using (System.Data.SqlClient.SqlBulkCopy bulkcopy = new System.Data.SqlClient.SqlBulkCopy(constr))
            {
                //1 指定数据插入目标表名称  
                bulkcopy.DestinationTableName = TableName;
                bulkcopy.BulkCopyTimeout = 660;
                //2 告诉SqlBulkCopy对象 内存表中的 OrderNO1和Userid1插入到OrderInfos表中的哪些列中
                foreach (DataColumn dc in dt.Columns)
                {
                    bulkcopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }
                //3 将内存表dt中的数据一次性批量插入到OrderInfos表中  
                bulkcopy.WriteToServer(dt);
            }
            return list.Count;
        }


        /// <summary>
        /// 将对象集合转换成Table
        /// 20180810 fj
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">对象集合</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            //获取对象所属类的属性集合
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            //创建空的DataTable对象
            DataTable table = new DataTable(typeof(T).Name);
            //新创建DataTable根据对象所属类创建列，并指定列数据类型
            foreach (PropertyDescriptor prop in properties)
            {
                Type type = prop.PropertyType;
                //处理Null类型
                if ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    type = type.GetGenericArguments()[0];
                }
                table.Columns.Add(prop.Name, type);
            }
            //将对象赋值到新创建DataTable对象中
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// 批量更新数据,列名不区分大小写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体集合</param>
        /// <param name="keyColumns">主键集合非空</param>
        /// <param name="columns">需要更新哪些列，为null是更新所有列</param>
        /// <param name="onRelations">跟新条件，默认是主键，多个条件关系是并列的and</param>
        public static int BatchUpdate<T>(List<T> list, List<string> keyColumns, IEnumerable<string> columns, Dictionary<string, string> onRelations = null,string tblName="")
        {
            int result = 0;
            if (list.Any())
            {
                Type type = typeof(T);
                DataTable pdco = null;
                var sbUpdateColumns = new StringBuilder();
                var sbOnRelation = new StringBuilder();
                if (string.IsNullOrEmpty(tblName))
                {
                    pdco = ClassHelper.ForType(type, keyColumns);
                }
                else
                {
                    pdco = ClassHelper.ForType(type, keyColumns,tblName);
                }
                var dt = new DataTable();
                var tableName = pdco.TableName;
                var columnsIndex = 0;
                var onRelationIndex = 0;
                var tempTableName = string.Empty;
                //构建需要更新的列
                if (columns == null)//更新除主键之外的列
                {
                    foreach (DataColumn i in pdco.Columns)
                    {
                        if (!pdco.PrimaryKey[0].ColumnName.Equals(i.ColumnName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Build the sql
                            if (columnsIndex > 0)
                                sbUpdateColumns.Append(", ");
                            sbUpdateColumns.AppendFormat("T.{0} = Temp.{0}", i.ColumnName);
                            columnsIndex++;
                        }

                    }
                }
                else//更新除主键之外的指定列
                {
                    foreach (var colname in columns)
                    {
                        var pc = pdco.Columns[colname];

                        // Build the sql
                        if (columnsIndex > 0)
                            sbUpdateColumns.Append(", ");
                        sbUpdateColumns.AppendFormat("T.{0} = Temp.{0}", colname);
                        columnsIndex++;
                    }
                }

                //构建更新条件
                if (onRelations == null)
                {
                    sbOnRelation.AppendFormat("T.{0} = Temp.{0}", pdco.PrimaryKey[0].ColumnName);
                }
                else
                {
                    foreach (var onRelation in onRelations)
                    {
                        if (onRelationIndex > 0)
                            sbOnRelation.Append(" AND ");
                        sbOnRelation.AppendFormat("T.{0} = Temp.{1}", onRelation.Key, onRelation.Value);
                        onRelationIndex++;
                    }
                }

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand command = new SqlCommand("", conn))
                    {
                        try
                        {
                            conn.Open();
                            //tempTableName = $"Temp{DateTime.Now.ToString("yyyMMddHHmmss")}{new Random().Next(1,10)}";
                            tempTableName = $"Temp{Guid.NewGuid().ToString("N")}";

                            //构建临时表
                            command.CommandText = $"SELECT * INTO {tempTableName} FROM {tableName} WHERE 1 = 2;";
                            command.ExecuteNonQuery();


                            
                            //list.ToDataTable(a => new object[] { list });

                            //插入临时表
                            dt= list.ToDataTable(a => new object[] { list });

                            //dt = ConvertToDataTable(list);
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(constr, SqlBulkCopyOptions.KeepIdentity))
                            {
                                bulkCopy.DestinationTableName = tempTableName;
                                bulkCopy.BatchSize = list.Count;
                                if (dt != null && dt.Rows.Count != 0)
                                {
                                    bulkCopy.WriteToServer(dt);
                                    bulkCopy.Close();
                                }
                            }

                            //从临时表更新到原表，并删除临时表
                            command.CommandTimeout = 300;
                            command.CommandText = string.Format($"UPDATE T SET {sbUpdateColumns.ToString()} FROM {pdco.TableName} T INNER JOIN {tempTableName} Temp ON {sbOnRelation.ToString()};DROP TABLE {tempTableName};");
                            result = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            if (!string.IsNullOrEmpty(tempTableName))
                            {
                                command.CommandText = string.Format($"DROP TABLE {tempTableName};");
                                result = command.ExecuteNonQuery();
                            }
                            throw ex;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            return result;
        }

        #endregion

    }
}
