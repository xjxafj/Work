using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
//using ODAC = Oracle.DataAccess.Client;
///


/// Oracle数据库操作类
///
public static class OracleHelper
{
    public static string connStr = "";

    //public static void BulkToDB(DataTable dt, string targetTable)
    //{
    //    string connOrcleString = "Data Source=;User Id=;Password=;";
    //    string err = "大批量插入时产生错误";
    //    OracleConnection conn = new OracleConnection(connOrcleString);
    //    if (conn.State != ConnectionState.Open)
    //    { conn.Open(); }
    //    OracleBulkCopy bulkCopy = new OracleBulkCopy(conn, Oracle.DataAccess.Client.OracleBulkCopyOptions.Default);
    //    bulkCopy.BatchSize = 100000;
    //    bulkCopy.BulkCopyTimeout = 260;
    //    //targetTable目标表名  
    //    bulkCopy.DestinationTableName = targetTable;

    //    try
    //    {
    //        if (conn.State != ConnectionState.Open)
    //        {
    //            conn.Open();
    //        }
    //        // conn.Open();    
    //        if (dt != null && dt.Rows.Count != 0)
    //        {
    //            bulkCopy.WriteToServer(dt);

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        conn.Close();
    //        if (bulkCopy != null)
    //            bulkCopy.Close();
    //    }
    //}

    public static DataTable ListToDataTable<T>(List<T> entitys)
    {
        //检查实体集合不能为空  
        if (entitys == null || entitys.Count < 1)
        {
            throw new Exception("需转换的集合为空");
        }
        //取出第一个实体的所有Propertie  
        Type entityType = entitys[0].GetType();
        PropertyInfo[] entityProperties = entityType.GetProperties();

        //生成DataTable的structure  
        //生产代码中，应将生成的DataTable结构Cache起来，此处略  
        DataTable dt = new DataTable();
        for (int i = 0; i < entityProperties.Length; i++)
        {
            //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);  
            dt.Columns.Add(entityProperties[i].Name);
        }
        //将所有entity添加到DataTable中  
        foreach (object entity in entitys)
        {
            //检查所有的的实体都为同一类型  
            if (entity.GetType() != entityType)
            {
                throw new Exception("要转换的集合元素类型不一致");
            }
            object[] entityValues = new object[entityProperties.Length];
            for (int i = 0; i < entityProperties.Length; i++)
            {
                entityValues[i] = entityProperties[i].GetValue(entity, null);
            }
            dt.Rows.Add(entityValues);
        }
        return dt;
    }


    //private static string connStr = "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = ORCL) ) );User Id = scott; Password=tiger;";
    /// <summary>  
    /// 执行数据库非查询操作,返回受影响的行数  
    /// </summary>  
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="cmdType">命令的类型</param>
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作影响的数据行数</returns>  
    public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        OracleCommand cmd = new OracleCommand();
        using (OracleConnection conn = new OracleConnection(connectionString))
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
    }

    /// <summary>  
    /// 执行数据库事务非查询操作,返回受影响的行数  
    /// </summary>  
    /// <param name="transaction">数据库事务对象</param>  
    /// <param name="cmdType">Command类型</param>  
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前事务查询操作影响的数据行数</returns>  
    public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        OracleCommand cmd = new OracleCommand();
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
        int val = 0;
        val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>  
    /// 执行数据库非查询操作,返回受影响的行数  
    /// </summary>  
    /// <param name="connection">Oracle数据库连接对象</param>  
    /// <param name="cmdType">Command类型</param>  
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作影响的数据行数</returns>  
    public static int ExecuteNonQuery(OracleConnection connection, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        if (connection == null)
            throw new ArgumentNullException("当前数据库连接不存在");
        OracleCommand cmd = new OracleCommand();
        PrepareCommand(cmd, connection, null, cmdType, cmdText, cmdParms);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>  
    /// 执行数据库事务非查询操作,返回受影响的行数  
    /// </summary>  
    /// <param name="transaction">数据库事务对象</param>  
    /// <param name="cmdType">Command类型</param>  
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前事务查询操作影响的数据行数</returns>  
    public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText, int count = 0, params OracleParameter[] cmdParms)
    {
        OracleCommand cmd = new OracleCommand();
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
        int val = 0;
        if (count != 0)
        {
            cmd.ArrayBindCount = count;
        }
        val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>  
    /// 执行数据库查询操作,返回OracleDataReader类型的内存结果集  
    /// </summary>  
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="cmdType">命令的类型</param>
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作返回的OracleDataReader类型的内存结果集</returns>  
    public static OracleDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        OracleCommand cmd = new OracleCommand();
        OracleConnection conn = new OracleConnection(connectionString);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return reader;
        }
        catch
        {
            cmd.Dispose();
            conn.Close();
            throw;
        }
    }

    /// <summary>  
    /// 执行数据库查询操作,返回DataSet类型的结果集  
    /// </summary>  
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="cmdType">命令的类型</param>
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作返回的DataSet类型的结果集</returns>  
    public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        OracleCommand cmd = new OracleCommand();
        OracleConnection conn = new OracleConnection(connectionString);
        DataSet ds = null;
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            OracleDataAdapter adapter = new OracleDataAdapter();
            adapter.SelectCommand = cmd;
            ds = new DataSet();
            adapter.Fill(ds);
            cmd.Parameters.Clear();
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }
        return ds;
    }


    /// <summary>  
    /// 执行数据库查询操作,返回DataSet类型的结果集  
    /// </summary>  
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="cmdType">命令的类型</param>
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作返回的DataSet类型的结果集</returns>  
    public static DataSet ExecuteDataSet(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        if (trans == null)
            throw new ArgumentNullException("当前数据库事务不存在");
        OracleConnection conn = trans.Connection;
        if (conn == null)
            throw new ArgumentException("当前事务所在的数据库连接不存在");
        DataSet ds = null;
        OracleCommand cmd = new OracleCommand();
        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
        OracleDataAdapter adapter = new OracleDataAdapter();
        adapter.SelectCommand = cmd;
        ds = new DataSet();
        adapter.Fill(ds);
        cmd.Parameters.Clear();
        return ds;
    }


    /// <summary>  
    /// 执行数据库查询操作,返回DataTable类型的结果集  
    /// </summary>  
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="cmdType">命令的类型</param>
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作返回的DataTable类型的结果集</returns>  
    public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        OracleCommand cmd = new OracleCommand();
        OracleConnection conn = new OracleConnection(connectionString);
        DataTable dt = null;

        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            OracleDataAdapter adapter = new OracleDataAdapter();
            adapter.SelectCommand = cmd;
            dt = new DataTable();
            adapter.Fill(dt);
            cmd.Parameters.Clear();
        }
        catch
        {
            throw;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }
        return dt;
    }

    /// <summary>  
    /// 执行数据库查询操作,返回DataTable类型的结果集  
    /// </summary>  
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="cmdType">命令的类型</param>
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作返回的DataTable类型的结果集</returns>  
    public static DataTable ExecuteDataTable(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        if (trans == null)
            throw new ArgumentNullException("当前数据库事务不存在");
        OracleConnection conn = trans.Connection;
        if (conn == null)
            throw new ArgumentException("当前事务所在的数据库连接不存在");
        OracleCommand cmd = new OracleCommand();
        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
        OracleDataAdapter adapter = new OracleDataAdapter();
        adapter.SelectCommand = cmd;
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        cmd.Parameters.Clear();
        return dt;
    }



    /// <summary>  
    /// 执行数据库查询操作,返回结果集中位于第一行第一列的Object类型的值  
    /// </summary>  
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="cmdType">命令的类型</param>
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作返回的结果集中位于第一行第一列的Object类型的值</returns>  
    public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        OracleCommand cmd = new OracleCommand();
        OracleConnection conn = new OracleConnection(connectionString);
        object result = null;
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            result = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
        }
        catch
        {
            throw;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }

        return result;
    }

    ///    <summary>  
    ///    执行数据库事务查询操作,返回结果集中位于第一行第一列的Object类型的值  
    ///    </summary>  
    ///    <param name="trans">一个已存在的数据库事务对象</param>  
    ///    <param name="commandType">命令类型</param>  
    ///    <param name="commandText">Oracle存储过程名称或PL/SQL命令</param>  
    ///    <param name="cmdParms">命令参数集合</param>  
    ///    <returns>当前事务查询操作返回的结果集中位于第一行第一列的Object类型的值</returns>  
    public static object ExecuteScalar(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        if (trans == null)
            throw new ArgumentNullException("当前数据库事务不存在");
        OracleConnection conn = trans.Connection;
        if (conn == null)
            throw new ArgumentException("当前事务所在的数据库连接不存在");
        OracleCommand cmd = new OracleCommand();
        object result = null;
        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
        result = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return result;
    }

    /// <summary>  
    /// 执行数据库查询操作,返回结果集中位于第一行第一列的Object类型的值  
    /// </summary>  
    /// <param name="conn">数据库连接对象</param>  
    /// <param name="cmdType">Command类型</param>  
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    /// <returns>当前查询操作返回的结果集中位于第一行第一列的Object类型的值</returns>  
    public static object ExecuteScalar(OracleConnection conn, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
    {
        if (conn == null) throw new ArgumentException("当前数据库连接不存在");
        OracleCommand cmd = new OracleCommand();
        object result = null;
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            result = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
        }
        catch
        {
            throw;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }
        return result;
    }

    /// <summary>  
    /// 执行数据库命令前的准备工作  
    /// </summary>  
    /// <param name="cmd">Command对象</param>  
    /// <param name="conn">数据库连接对象</param>  
    /// <param name="trans">事务对象</param>  
    /// <param name="cmdType">Command类型</param>  
    /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
    /// <param name="cmdParms">命令参数集合</param>  
    private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] cmdParms)
    {
        if (conn.State != ConnectionState.Open)
            conn.Open();
        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        if (trans != null)
            cmd.Transaction = trans;
        cmd.CommandType = cmdType;
        if (cmdParms != null)
        {
            foreach (OracleParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }

    /// <summary>  
    /// 将.NET日期时间类型转化为Oracle兼容的日期时间格式字符串  
    /// </summary>  
    /// <param name="date">.NET日期时间类型对象</param>  
    /// <returns>Oracle兼容的日期时间格式字符串（如该字符串：TO_DATE('2007-12-1','YYYY-MM-DD')）</returns>  
    public static string GetOracleDateFormat(DateTime date)
    {
        return "TO_DATE('" + date.ToString("yyyy-M-dd") + "','YYYY-MM-DD')";
    }

    /// <summary>  
    /// 将.NET日期时间类型转化为Oracle兼容的日期格式字符串  
    /// </summary>  
    /// <param name="date">.NET日期时间类型对象</param>  
    /// <param name="format">Oracle日期时间类型格式化限定符</param>  
    /// <returns>Oracle兼容的日期时间格式字符串（如该字符串：TO_DATE('2007-12-1','YYYY-MM-DD')）</returns>  
    public static string GetOracleDateFormat(DateTime date, string format)
    {
        if (format == null || format.Trim() == "") format = "YYYY-MM-DD";
        return "TO_DATE('" + date.ToString("yyyy-M-dd") + "','" + format + "')";
    }

    /// <summary>  
    /// 将指定的关键字处理为模糊查询时的合法参数值  
    /// </summary>  
    /// <param name="source">待处理的查询关键字</param>  
    /// <returns>过滤后的查询关键字</returns>  
    public static string HandleLikeKey(string source)
    {
        if (source == null || source.Trim() == "") return null;
        source = source.Replace("[", "[]]");
        source = source.Replace("_", "[_]");
        source = source.Replace("%", "[%]");
        return ("%" + source + "%");
    }

    public static bool ExcuteBulkData(string connectionString, DataTable dt)
    {
        bool result = false;
        dt = new DataTable();
        dt.Columns.Add("ID", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        using (OracleConnection conn = new OracleConnection(connectionString))
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.Default))
            {
               
                if (dt != null && dt.Rows.Count > 0)
                {
                    bulkCopy.DestinationTableName = dt.TableName;
                    foreach (DataColumn item in dt.Columns)
                    {
                        string col = item.ColumnName;
                        bulkCopy.ColumnMappings.Add(col, col);
                    }
                    conn.Open();
                    bulkCopy.WriteToServer(dt);
                    result = true;
                }
            }
        }
        return result;
    }
}