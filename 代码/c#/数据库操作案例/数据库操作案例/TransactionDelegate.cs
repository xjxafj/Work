using DataTool;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Documents;

namespace DataService.Common.DBHelper
{
    public class TransactionDelegate
    {
        public delegate void TransactionProcessDelegate(TransactionDelegate trd, dynamic dy); //声明了一个Delegate Type
        public event TransactionProcessDelegate ProcessEventHandler; //声明了一个Delegate对象
        public OracleConnection con = null;
        public int re = 0;
        public bool isCommit = false;
        public bool isHaveError = false;
        public OracleTransaction temTR { get; set; }
        public StringBuilder tempSql = new StringBuilder();
        public List temParams = new List();
        public DataSet temDs = null;
        /// <summary>
        /// 保存的结果
        /// </summary>
        public dynamic result = null;
        /// 开启事务操作
        ///
        public void Start()
        {
            this.isHaveError = false; //
            this.isCommit = false;
            this.con = new OracleConnection(OracleHelper.connStr);
            this.con.Open();
            this.temTR = con.BeginTransaction();
        }
        public void Commit()
        {
            this.isCommit = true;
            Close();
            Start();
        }

        public void Rollback()
        {
            this.isCommit = false;
            this.isHaveError = true;
            Close();
            Start();
        }
        /// <summary>
        /// 关闭并看是否提交或者回滚
        /// </summary>
        public void Close()
        {
            if (temTR != null && isCommit)
            {
                if (isCommit)
                {
                    temTR.Commit();
                }
                else
                {
                    temTR.Rollback();
                }
            }
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (temTR != null)
            {
                temTR.Dispose();
            }
        }
        /// <summary>
        /// 开始执行带事务的过程
        /// </summary>
        public void BeginTransaction(TransactionDelegate de, dynamic dy)
        {
            if (ProcessEventHandler != null)
            {
                try
                {
                    this.Start();
                    ProcessEventHandler.Invoke(this, dy);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    isCommit = false;
                    isHaveError = true;
                    throw;
                }
                finally
                {
                    this.Close();
                }
            }
        }
    }
}