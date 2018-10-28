using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data;
using System.Threading;
using System.ComponentModel;
using Oracle.ManagedDataAccess.Client;
using DataService.Common.DBHelper;
using System.Diagnostics;

namespace DataTool
{
    public class SaveHelper
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
       
        public string mess = string.Empty;

        public string SaveHelperStatus = string.Empty;

        public Stopwatch st1 = new Stopwatch();
        public Stopwatch st2 = new Stopwatch();


        public long saveTatol = 0;
        //提交Commit设置
        public int commitCount = 200000;
        public int strMaxLength = 40000;
        StringBuilder temSql = new StringBuilder();
        StringBuilder temSql1 = new StringBuilder();
        StringBuilder temSql2 = new StringBuilder();
        StringBuilder temSql3 = new StringBuilder();
        List<OracleParameter> temParam1 = new List<OracleParameter>();
        List<OracleParameter> temParam2 = new List<OracleParameter>();
        List<OracleParameter> temParam3 = new List<OracleParameter>();
        public int typeno = 0;
        int re = 0;
        public int cc = 0;
        public string FilePath1 { get; set; }
        public string FilePath2 { get; set; }

        public string fromLang = string.Empty;
        public string toLang = string.Empty;
        public int HW_RELEVANCE = 0;

      

        public string Original { get; set; }
        public bool isHaveError { get; set; }
        public string tableName { get; set; }

        public SaveParam p = null;

        public List<SaveParam> list = null;

        string loc = "1";


        public bool IsBusy = false;

        public SaveHelper(string filePath1, string filePath2)
        {

            if (!File.Exists(filePath1) || !File.Exists(filePath2))
            {
                MessageBox.Show("文件错误");
                isHaveError = true;
                return;
            }
            FilePath1 = filePath1;
            FilePath2 = filePath2;
        }

        public void ReadFile()
        {
            SaveHelperStatus = string.Format("选择的语言为：{0}_{1}", fromLang, toLang);
            
            if (isHaveError)
            {
                return;
            }
            FileStream fs1 = new FileStream(FilePath1, FileMode.Open);
            StreamReader reader1 = new StreamReader(fs1, UnicodeEncoding.UTF8);
            FileStream fs2 = new FileStream(FilePath2, FileMode.Open);
            StreamReader reader2 = new StreamReader(fs2, UnicodeEncoding.UTF8);
            cc = 0;
            list = new List<SaveParam>();
            string content1 = string.Empty;
            string content2 = string.Empty;
            string temStr = string.Empty;
            //// 一行一行读取 
            try
            {
                while ((content1 = reader1.ReadLine()) != null && (content2 = reader2.ReadLine()) != null)
                {
                    cc++;
                    if (!string.IsNullOrEmpty(content1) && !string.IsNullOrEmpty(content2) && content1.Length <= strMaxLength && content2.Length <= strMaxLength)
                    {
                        //md5 = Common.EncryptMD5(content1);
                        p = new SaveParam() { Content1 = content1, fromLang = fromLang, Content2 = content2, toLang = toLang };
                        list.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                isHaveError = true;
                SaveHelperStatus = ex.Message;
            }
            finally
            {
                if (fs1 != null)
                {
                    fs1.Close();
                }
                if (fs2 != null)
                {
                    fs2.Close();
                }
                if (reader1 != null)
                {
                    reader1.Close();
                }
                if (reader2 != null)
                {
                    reader2.Close();
                }
              
            }
            //剩余的更新
            int count = list.Count();
            if (count > 0)
            {
                lock (loc)
                {
                    saveTatol = count;
                }
                UpdateDB(list);
            }
        }

        public void UpdateDB(List<SaveParam> p)
        {
            st2.Reset();
            TransactionDelegate t = new TransactionDelegate();
            t.ProcessEventHandler += AddList_ProcessEventHandler;
            t.BeginTransaction(t, p);
        }



       
        List<string> guidList = new List<string>();
        List<string> guidList1 = new List<string>();
        List<string> guidList2 = new List<string>();
        List<string> contentList1 = new List<string>();
        List<string> contentList2 = new List<string>();
        List<string> originalList = new List<string>();
        List<int> HW_RelevanceList = new List<int>();
        private void AddList_ProcessEventHandler(TransactionDelegate trd, dynamic dy)
        {
            string temStr = string.Empty;
            List<SaveParam> p = (List<SaveParam>)dy;
            if (this.isHaveError)
            {
                return;
            }
            re = 0;
            int count = 0;
            foreach (SaveParam item in p)
            {
                if (this.isHaveError)
                {
                    return;
                }
                count++;
                if (string.IsNullOrEmpty(item.Content1) || string.IsNullOrEmpty(item.Content2) || item.Content1.Length > strMaxLength || item.Content1.Length > strMaxLength)
                {
                    continue;
                }
                guidList.Add(item.guid);
                guidList1.Add(item.guid1);
                guidList2.Add(item.guid2);
                contentList1.Add(item.Content1);
                contentList2.Add(item.Content2);
                originalList.Add(this.Original);
                HW_RelevanceList.Add(this.HW_RELEVANCE);
                if (count % commitCount == 0 && guidList.Count > 0)
                {
                    SaveHelperStatus = string.Format("布隆耗时{0}:{1}s,执行SQL中", guidList.Count.ToString(), (st2.ElapsedMilliseconds).ToString());
                    NewMethod(trd);
                }
            }
            if (guidList.Count > 0)
            {
                SaveHelperStatus = string.Format("最后一次布隆耗时{0}:{1}s,执行SQL中", guidList.Count.ToString(), (st2.ElapsedMilliseconds).ToString());
                NewMethod(trd);
            }
            p.Clear();
            IsBusy = false;
        }

       

        private void ClearParam()
        {
            temSql1.Clear();
            temSql2.Clear();
            temSql3.Clear();
            temParam1.Clear();
            temParam2.Clear();
            temParam3.Clear();
            guidList.Clear();
            guidList1.Clear();
            guidList2.Clear();
            contentList1.Clear();
            contentList2.Clear();
            originalList.Clear();
            HW_RelevanceList.Clear();
        }

        private void NewMethod(TransactionDelegate trd)
        {
            try
            {
                if (isHaveError)
                {
                    return;
                }
                st1.Restart();
                //SaveHelperStatus = string.Format("开始执行SQL");
                if (trd != null && trd.isHaveError)
                {
                    return;
                }
                temSql1.AppendFormat(" insert into MONOLINGUALSENTENCE{0}(ID,Content,Original,HW_Relevance) values(:ID,:Content,:Original,:HW_Relevance)", fromLang);
                temParam1.Add(new OracleParameter(":ID", OracleDbType.Varchar2, guidList1.ToArray(), System.Data.ParameterDirection.Input));
                temParam1.Add(new OracleParameter(":Content", OracleDbType.Varchar2, contentList1.ToArray(), System.Data.ParameterDirection.Input));
                temParam1.Add(new OracleParameter(":Original", OracleDbType.Varchar2, originalList.ToArray(), System.Data.ParameterDirection.Input));
                temParam1.Add(new OracleParameter(":HW_Relevance", OracleDbType.Int32, HW_RelevanceList.ToArray(), System.Data.ParameterDirection.Input));

                temSql2.AppendFormat(" insert into MONOLINGUALSENTENCE{0}(ID,Content,Original,HW_Relevance) values(:ID,:Content,:Original,:HW_Relevance)", toLang);
                temParam2.Add(new OracleParameter(":ID", OracleDbType.Varchar2, guidList2.ToArray(), System.Data.ParameterDirection.Input));
                temParam2.Add(new OracleParameter(":Content", OracleDbType.Varchar2, contentList2.ToArray(), System.Data.ParameterDirection.Input));
                temParam2.Add(new OracleParameter(":Original", OracleDbType.Varchar2, originalList.ToArray(), System.Data.ParameterDirection.Input));
                temParam2.Add(new OracleParameter(":HW_Relevance", OracleDbType.Int32, HW_RelevanceList.ToArray(), System.Data.ParameterDirection.Input));

                string temStr1 = string.Format(":{0}ID", fromLang);
                string temStr2 = string.Format(":{0}ID", toLang);
                temSql3.AppendFormat(" insert into BILINGUALSENTENCE{0}{1}(ID,{0}ID,{1}ID) values(:ID,{2},{3}) ", fromLang, toLang, temStr1, temStr2);
                temParam3.Add(new OracleParameter(":ID", OracleDbType.Varchar2, guidList.ToArray(), System.Data.ParameterDirection.Input));
                temParam3.Add(new OracleParameter(temStr1, OracleDbType.Varchar2, guidList1.ToArray(), System.Data.ParameterDirection.Input));
                temParam3.Add(new OracleParameter(temStr2, OracleDbType.Varchar2, guidList2.ToArray(), System.Data.ParameterDirection.Input));
                SaveHelperStatus = string.Format("开始保存语言{0}:{1}", fromLang, guidList.ToArray());
                re = OracleHelper.ExecuteNonQuery(trd.temTR, CommandType.Text, temSql1.ToString(), guidList.Count, temParam1.ToArray());
                if (re > 0)
                {
                    SaveHelperStatus = string.Format("保存语言{0}成功:{1}", fromLang, re);
                    re = OracleHelper.ExecuteNonQuery(trd.temTR, CommandType.Text, temSql2.ToString(), guidList.Count, temParam2.ToArray());
                    if (re > 0)
                    {
                        SaveHelperStatus = string.Format("保存语言{0}成功:{1}", toLang, re);
                        re = OracleHelper.ExecuteNonQuery(trd.temTR, CommandType.Text, temSql3.ToString(), guidList.Count, temParam3.ToArray());
                        if (re > 0)
                        {
                            SaveHelperStatus = string.Format("保存语言{0}_{1}成功:{2}", fromLang, toLang, re);
                            if (trd.temTR != null)
                            {
                                try
                                {
                                    trd.Commit();
                                    
                                    lock (loc)
                                    {
                                        saveTatol -= re;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SaveHelperStatus = string.Format("保存语言{0}_{1}失败:{2}", fromLang, toLang, guidList.Count);
                                    //MessageBox.Show("最后执行commit时出现错误了"+ex.Message);
                                    isHaveError = true;
                                    trd.Rollback();
                                }

                            }
                            else
                            {
                                isHaveError = true;
                                trd.Rollback();
                            }
                        }
                    }
                    else
                    {
                        SaveHelperStatus = string.Format("保存语言{0}失败:{1}", toLang, guidList.Count);
                        isHaveError = true;
                        trd.Rollback();
                    }
                }
                else
                {
                    SaveHelperStatus = string.Format("保存语言{0}失败:{1}", fromLang, guidList.Count);
                    isHaveError = true;
                    trd.Rollback();
                }
            }
            catch (Exception ex)
            {
                isHaveError = true;
                SaveHelperStatus = ex.Message;
            }
            if (!isHaveError)
            {
                SaveHelperStatus = string.Format("执行{0}:{1}s", guidList.Count().ToString(), (st1.ElapsedMilliseconds / (double)1000).ToString());

            }
            else
            {
                SaveHelperStatus = string.Format("SQL执行出现了错误{0}:{1}s", SaveHelperStatus, (st1.ElapsedMilliseconds / (double)1000).ToString());
            }
            ClearParam();
            st2.Reset();
        }
    }

    public class SaveParam
    {
        private string content1 = string.Empty;
        public string Content1
        {
            get
            {
                return content1;
            }

            set
            {
                content1 = value;
            }
        }

        public string guid = Guid.NewGuid().ToString();


        public string guid1 = Guid.NewGuid().ToString();

        public string fromLang = string.Empty;


        private string content2 = string.Empty;

        public string Content2
        {
            get
            {
                return content2;
            }

            set
            {
                content2 = value;
            }
        }

        public string toLang = string.Empty;

        public string guid2 = Guid.NewGuid().ToString();
    }
}