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
    public partial class DALT_Event_StuClassTask
    {
        public List<T_Event_StuClassTask> GetAllList(string where = "")
        {
            DataSet ds = GetList(where);

            List<T_Event_StuClassTask> lst = new List<T_Event_StuClassTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        public bool DeleteWhere(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_Event_StuClassTask ");
            strSql.Append(" where " + where);
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int AddPartTask(int taskid,string[] ids)
        {
            DALT_Event_StuClassTask dal = new DALT_Event_StuClassTask();

            foreach(string id in ids)
            {
                T_Event_StuClassTask item = new T_Event_StuClassTask();
                item.ClassTaskId = taskid;
                item.StuId = Convert.ToInt32(id);

                dal.Add(item);

            }

            return 1;
        }

        public int EditPartTask(int taskid,string[] ids)
        {
            DALT_Event_StuClassTask dal = new DALT_Event_StuClassTask();

            string where = "ClassTaskId = " + taskid;
            dal.DeleteWhere(where);

            foreach (string id in ids)
            {
                T_Event_StuClassTask item = new T_Event_StuClassTask();
                item.ClassTaskId = taskid;
                item.StuId = Convert.ToInt32(id);

                dal.Add(item);

            }

            return 1;
        }

        //获取所有该班级日程的学生
        public string GetStus(int taskid)
        {
            DALT_Event_StuClassTask dal = new DALT_Event_StuClassTask();

            string where = "ClassTaskId = " + taskid;
            List<T_Event_StuClassTask> lst =  dal.GetAllList(where);

            string res = "";

            foreach(T_Event_StuClassTask item in lst)
            {
                res += item.StuId + ",";
            }

            res = res.Substring(0, res.LastIndexOf(","));

            return res;
        }
    }
}
