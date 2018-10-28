using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
    
namespace 数据库操作案例
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder temSql = new StringBuilder();
            List<OracleParameter> temParams = new List<OracleParameter>();
            temSql.AppendFormat(" select '' from dual;");
           object  t= OracleHelper.ExecuteScalar(OracleHelper.connStr, System.Data.CommandType.Text, temSql.ToString(), temParams.ToArray());
        }
    }
}
