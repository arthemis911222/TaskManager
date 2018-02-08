using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DAL
{
    public partial class DALT_Event_MyTask
    {
        public List<T_Event_MyTask> GetAllList(string where="")
        {
            DataSet ds = GetList(where);

            List<T_Event_MyTask> lst = new List<T_Event_MyTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        public DataSet GetList(DateTime datetime, int id)                        //wx
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,StartTime,EndTime,Type,Description,IsAlert,AlertTime,State,StuId ");
            strSql.Append(" FROM T_Event_MyTask ");

            //格式转换
            String afterDate = datetime.ToString("yyyy/MM/dd");

            if (afterDate != "")
            {
                strSql.Append(" where " + "(Cast(StartTime as datetime) between '" + afterDate + " 0:00:00' and '" + afterDate + "  23:59:59' )And StuId=" + id);

            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public List<T_Event_MyTask> FindList(DateTime datetime, int id)
        {
            DataSet ds = GetList(datetime, id);

            List<T_Event_MyTask> lst = new List<T_Event_MyTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }
    }
}
