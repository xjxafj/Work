using ProjectAutomationCreateWeb.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ProjectAutomationCreateWeb.Helper
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// 打开邮件
        /// </summary>
        private void OpenEmail()
        {
            try
            {
                // StartProcess("aboutUswebsite");

                //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
                //发送地址 
                string to = "tsctool@huawei.com";
                //主题 
                string subject = "Feedback to MagicKit client";
                //subject = HttpUtility.UrlEncode(subject, enc);
                //内容
                string body = "";
                //body = HttpUtility.UrlEncode(body, enc);//
                //打开标准的软件客户端
                Process.Start(string.Format("mailto:{0}?subject={1}&body={2}", to, subject, body));
            }
            catch
            { }
        }

        private static Encoding myEncoding;

        /// <summary>
        /// 自定义服务编码
        /// </summary>
        public static Encoding MyEncoding
        {
            get
            {
                if (myEncoding == null)
                {
                    myEncoding = Encoding.UTF8;
                }
                return myEncoding;
            }
            set
            {
                myEncoding = value;
            }
        }

        private static EmailContext emailContext = null;
        /// <summary>
        /// 构建发送人上下文对象
        /// </summary>
        public static EmailContext EmailContext
        {
            get
            {
                if (emailContext == null)
                {
                    try
                    {
                        emailContext = new EmailContext();
                        emailContext.SmtpServer = ConfigConstStr.EmailServerName;
                        emailContext.Port = ConfigConstStr.EmailPort;
                        emailContext.EnableSSL = false;
                        emailContext.UserName = ConfigConstStr.EmailUserName;
                        emailContext.Password = ConfigConstStr.EmailPassWord;
                        emailContext.Default = string.IsNullOrEmpty(emailContext.UserName);
                    }
                    catch (Exception ex) { }
                }
                return emailContext;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="FromAddress">来自邮件地址</param>
        /// <param name="ToAddress">发送目标邮件地址</param>
        /// <param name="CCUserAddress">抄送</param>
        /// <param name="FromDisplayName"></param>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="smtp">发送邮件上下文对象</param>
        public static void SendMail(string FromAddress, string ToAddress, string[] CCUserAddress, string FromDisplayName, string Subject, string Body, EmailContext smtp = null)
        {
            if (smtp == null)
                smtp = EmailContext;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(FromAddress, FromDisplayName, MyEncoding);//发件人地址，发件人姓名，编码   
            msg.To.Add(ToAddress);
            if (CCUserAddress != null && CCUserAddress.Length > 0)
            {
                foreach (string cc in CCUserAddress)
                {
                    msg.CC.Add(cc);
                }
            }
            msg.Subject = Subject;    //邮件标题
            msg.SubjectEncoding = MyEncoding;
            msg.Body = Body;    //邮件内容   
            msg.BodyEncoding = MyEncoding;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            SmtpClient client = new SmtpClient();
            client.Host = smtp.SmtpServer;
            if (smtp.Port != 0)
            {
                client.Port = smtp.Port;//如果不指定端口，默认端口为25
            }
            if (smtp.Default)
            {
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                client.Credentials = new System.Net.NetworkCredential(smtp.UserName, smtp.Password);//向SMTP服务器提交认证信息
            }
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = smtp.EnableSSL;//默认为false
            object userState = msg;
            try
            {
                //client.SendAsync(msg, userState);   //异步发送
                client.Send(msg); //同步发送 
            }
            catch (System.Net.Mail.SmtpException se)
            {
                throw;
            }
        }

        /// <summary>
        /// 发送永久
        /// </summary>
        /// <param name="ToUserName">发送的指定用户</param>
        /// <param name="FromDisName">发件人名称</param>
        /// <param name="Subject">邮件主题</param>
        /// <param name="Body">邮件内容</param>
        public static void SendEmailFromPublicToUser(string ToUserName, string FromDisName = "TscTool", string Subject = "你有新的翻译订单", string Body = "")
        {
            string from = ConfigConstStr.PublicEmailAddr;
            try
            {
                //string[] CCUserNames = new string[] { ".xie@.com","@notesmail..com.cn" };
                string[] CCUserNames = new string[] { "fwx220230@notesmail.huawei.com.cn" };
                SendMail(from, ToUserName, CCUserNames, FromDisName, Subject, Body);
            }
            catch (Exception)
            {
            }
        }

    }
    /// <summary>
    /// Eail上下文
    /// </summary>
    [Serializable]
    public class EmailContext
    {

        private string _smtpServer;

        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        private int _port = 25;

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private bool _enableSSL = false;

        public bool EnableSSL
        {
            get { return _enableSSL; }
            set { _enableSSL = value; }
        }

        private bool _default;

        public bool Default
        {
            get { return _default; }
            set { _default = value; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

    }
}
