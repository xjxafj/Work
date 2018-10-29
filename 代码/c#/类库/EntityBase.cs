using DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class User : EntityBase
    {
        public User()
        {
            setTableInfo("User","用户表");
        }

        public Guid Id
        {
            get
            {
                return getProperty<Guid>("Id");
            }
            set
            {
                setProperty("Id", value, System.Data.DbType.String, 50);
            }
        }


        public string Name
        {
            get
            {
                return getProperty<string>("Name");
            }
            set
            {
                setProperty("Name", value, System.Data.DbType.String,2);
            }
        }


        public int Age
        {
            get
            {
                return getProperty<int>("Age");
            }
            set
            {
                setProperty("Age", value, System.Data.DbType.Int32);
            }
        }


        public DateTime CreateTime
            
        {
            get
            {
                return getProperty<DateTime>("CreateTime");
            }
            set
            {
                setProperty("CreateTime", value, System.Data.DbType.Int32);
            }
        }

    }
}
