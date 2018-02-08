using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DAL
{
    public partial class DALT_Event_ClassTask
    {
        public List<T_Event_ClassTask> GetAllList(string where = "")
        {
            DataSet ds = GetList(where);

            List<T_Event_ClassTask> lst = new List<T_Event_ClassTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        //判断是否可以修改or删除
        public int CanEdit(int userLevel,int userid,int ctid)
        {
            List<T_Event_ClassTask> lst;
            if (userLevel == 0)
                lst = SearchStu(userid, ctid);
            else
                lst = SearchTea(userid, ctid);

            if (lst.Count() == 0)
                return 0;
            else
                return 1;
        }

        public List<T_Event_ClassTask> SearchStu(int stuid,int ctid)
        {
            string where = "Id = " + ctid + " and StuId = " + stuid;
            DataSet ds = GetListByVStu(where);

            List<T_Event_ClassTask> lst = new List<T_Event_ClassTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;

        }
        public List<T_Event_ClassTask> SearchTea(int teaid, int ctid)
        {
            string where = "Id = " + ctid + " and TeaId = " + teaid;
            DataSet ds = GetListByVTea(where);

            List<T_Event_ClassTask> lst = new List<T_Event_ClassTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;

        }

        public DataSet GetListByVStu(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,StartTime,EndTime,Type,Description,IsAlert,AlertTime,State,ClassId,WPeople,IsAllStuTask ");
            strSql.Append(" FROM V_ClassTask_Stu ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetListByVTea(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,StartTime,EndTime,Type,Description,IsAlert,AlertTime,State,ClassId,WPeople,IsAllStuTask ");
            strSql.Append(" FROM V_ClassTask_Tea ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        //自定义添加，发布人信息-学生
        public int AddTaskStu(T_Event_ClassTask item,int userid)
        {
            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();

            int taskid = dal.Add(item);

            if (taskid == 0)
                return 0;

            #region 发布人信息添加
            DALT_ClassTask_Stu cs_dal = new DALT_ClassTask_Stu();
            T_ClassTask_Stu cs_item = new T_ClassTask_Stu();
            cs_item.ClassId = taskid;
            cs_item.StuId = userid;
            cs_dal.Add(cs_item);
            #endregion

            return taskid;
        }

        //自定义添加，发布人信息-老师
        public int AddTaskTea(T_Event_ClassTask item, int userid)
        {
            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();

            int taskid = dal.Add(item);

            if (taskid == 0)
                return 0;

            #region 发布人信息添加(Tea)
            DALT_ClassTask_Tea cs_dal = new DALT_ClassTask_Tea();
            T_ClassTask_Tea cs_item = new T_ClassTask_Tea();
            cs_item.ClassId = taskid;
            cs_item.TeaId = userid;
            cs_dal.Add(cs_item);
            #endregion

            return taskid;
        }

        public DataSet GetList(DateTime datetime, int id)                        //wx
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,StartTime,EndTime,Type,Description,IsAlert,AlertTime,State,ClassId ,WPeople,IsAllStuTask,StuId");
            strSql.Append(" FROM V_ClassTask_Stu ");

            //格式转换
            String afterDate = datetime.ToString("yyyy/MM/dd");

            if (afterDate != "")
            {
                strSql.Append(" where " + "(Cast(StartTime as datetime) between '" + afterDate + " 0:00:00' and '" + afterDate + "  23:59:59'  )And StuId=" + id);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public List<T_Event_ClassTask> FindList(DateTime datetime, int id)
        {
            DataSet ds = GetList(datetime, id);

            List<T_Event_ClassTask> lst = new List<T_Event_ClassTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        public DataSet GetListT(DateTime datetime, int id)                        //wx
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,StartTime,EndTime,Type,Description,IsAlert,AlertTime,State,ClassId ,WPeople,IsAllStuTask,TeaId");
            strSql.Append(" FROM V_ClassTask_Tea ");

            //格式转换
            String afterDate = datetime.ToString("yyyy/MM/dd");

            if (afterDate != "")
            {
                strSql.Append(" where " + "(Cast(StartTime as datetime) between '" + afterDate + " 0:00:00' and '" + afterDate + "  23:59:59'  )And TeaId=" + id);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public List<T_Event_ClassTask> FindListT(DateTime datetime, int id)
        {
            DataSet ds = GetListT(datetime, id);

            List<T_Event_ClassTask> lst = new List<T_Event_ClassTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        //王一情
        //先通过教师的ID找到所选学生的班级，然后找到这个班级的班级日程
        public List<T_Event_ClassTask> GetTeaClassList2(int TeaId)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select ClassId from T_Task_Course where TeaId=" + TeaId;

            co.Open();
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            List<T_Event_ClassTask> list = new List<T_Event_ClassTask>();
            while (dr.Read())
            {
                int ClassId = Convert.ToInt32(dr["ClassId"]);
                DataSet ds = GetList("ClassId=" + "'" + ClassId + "'");
                foreach (DataRow dd in ds.Tables[0].Rows)
                {
                    list.Add(DataRowToModel(dd));
                }
            }
            dr.Close();
            co.Close();
            return list;
        }
    }
}
