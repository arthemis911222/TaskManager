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
    public partial class DALT_Event_CourseTask
    {
        public List<T_Event_CourseTask> GetAllList(string where = "")
        {
            DataSet ds = GetList(where);

            List<T_Event_CourseTask> lst = new List<T_Event_CourseTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        //王一情
        public List<T_Event_CourseTask> GetTeaCourseList(string where)
        {
            DataSet ds = GetList(where);

            List<T_Event_CourseTask> lst = new List<T_Event_CourseTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        //王一情  先通过学生的ID找到所选的课程号，然后找到这个课程的课程日程
        public List<T_Event_CourseTask> GetStuCourseList(int stuId)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select CourseId from T_Task_Choose where StuId=" + stuId;

            co.Open();
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            List<T_Event_CourseTask> list = new List<T_Event_CourseTask>();
            while (dr.Read())
            {
                int CourseId = Convert.ToInt32(dr["CourseId"]);
                DataSet ds = GetList("CourseId=" + "'" + CourseId + "'");
                foreach (DataRow dd in ds.Tables[0].Rows)
                {
                    list.Add(DataRowToModel(dd));
                }
            }
            dr.Close();
            co.Close();
            return list;
        }

        //吴佳洁-搜索
        public List<T_Search_Event> GetAllSearch(int type,string where = "")
        {
            DataSet ds = new DataSet();
            if (type == 0)
                ds = GetListByViewStu(where);
            else if(type == 1)
                ds = GetListByViewTea(where);

            List<T_Search_Event> lst = new List<T_Search_Event>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToSearch(dr));
            }

            return lst;
        }

        public DataSet GetListByViewStu(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,StartTime,EndTime,Type,Description,StartWeek,IsAlert");
            strSql.Append(" FROM V_Search_CourseTaskStu ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetListByViewTea(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,StartTime,EndTime,Type,Description,StartWeek,IsAlert");
            strSql.Append(" FROM V_Search_CourseTaskTea ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public T_Search_Event DataRowToSearch(DataRow row)
        {
            T_Search_Event model = new T_Search_Event();

            if (row != null)
            {

                DateTime st = (DateTime)row["StartWeek"];
                string ym = st.ToString("yyyy-MM-dd");

                if (row["StartTime"] != null)
                {
                    string strSt = row["StartTime"].ToString();
                    string res = ym + " " + strSt;
                    DateTime newSt = Convert.ToDateTime(strSt);
                    model.StartTime = newSt;
                }
                if (row["EndTime"] != null)
                {
                    string strEn = row["EndTime"].ToString();
                    string res = ym + " " + strEn;
                    DateTime newEn = Convert.ToDateTime(strEn);
                    model.EndTime = newEn;
                }
                if (row["Name"] != null && row["Name"].ToString() != "")
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["IsAlert"] != null && row["IsAlert"].ToString() != "")
                {
                    model.IsAlert = int.Parse(row["IsAlert"].ToString());
                }
            }
            return model;
        }

        public DataSet GetList(DateTime datetime, int id)                        //wx
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM V_CourseTask_Stu ");

            //格式转换
            String afterDate = datetime.ToString("yyyy/MM/dd");

            if (afterDate != "")
            {
                strSql.Append(" where " + "(Cast(StartWeek as datetime) between '" + afterDate + " 0:00:00' and '" + afterDate + "  23:59:59' ) And StuId=" + id);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public List<T_Event_CourseTask> FindList(DateTime datetime, int id)
        {
            DataSet ds = GetList(datetime, id);

            List<T_Event_CourseTask> lst = new List<T_Event_CourseTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModelWx(dr));
            }

            return lst;
        }

        public DataSet GetListT(DateTime datetime, int id)                        //wx
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM V_CourseTask_Tea ");

            //格式转换
            String afterDate = datetime.ToString("yyyy/MM/dd");

            if (afterDate != "")
            {
                strSql.Append(" where " + "(Cast(StartWeek as datetime) between '" + afterDate + " 0:00:00' and '" + afterDate + "  23:59:59'  )And TeaId=" + id);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public List<T_Event_CourseTask> FindListT(DateTime datetime, int id)
        {
            DataSet ds = GetListT(datetime, id);

            List<T_Event_CourseTask> lst = new List<T_Event_CourseTask>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModelWx(dr));
            }

            return lst;
        }

        public TaskManager.Model.T_Event_CourseTask DataRowToModelWx(DataRow row)
        {
            TaskManager.Model.T_Event_CourseTask model = new TaskManager.Model.T_Event_CourseTask();
            if (row != null)
            {
                if (row["Id"] != null && row["Id"].ToString() != "")
                {
                    model.Id = int.Parse(row["Id"].ToString());
                }
                if (row["StartTime"] != null)
                {
                    model.StartTime = row["StartTime"].ToString();
                }
                if (row["EndTime"] != null)
                {
                    model.EndTime = row["EndTime"].ToString();
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["StartWeek"] != null && row["StartWeek"].ToString() != "")
                {
                    model.StartWeek = DateTime.Parse(row["StartWeek"].ToString());
                }
                if (row["IsAlert"] != null && row["IsAlert"].ToString() != "")
                {
                    model.IsAlert = int.Parse(row["IsAlert"].ToString());
                }
                if (row["State"] != null && row["State"].ToString() != "")
                {
                    model.State = int.Parse(row["State"].ToString());
                }
                if (row["CourseId"] != null && row["CourseId"].ToString() != "")
                {
                    model.CourseId = int.Parse(row["CourseId"].ToString());
                }
                if (row["WPeople"] != null)
                {
                    model.WPeople = row["WPeople"].ToString();
                }
            }
            return model;
        }

        //王一情
        //先通过教师的ID找到所任课的课程，然后找到这些课程的课程日程
        public List<T_Event_CourseTask> GetTeaCourseList2(int TeaId)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select Id from T_Task_Course where TeaId=" + TeaId;

            co.Open();
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            List<T_Event_CourseTask> list = new List<T_Event_CourseTask>();
            while (dr.Read())
            {
                int CourseId = Convert.ToInt32(dr["Id"]);
                DataSet ds = GetList("CourseId=" + "'" + CourseId + "'");
                foreach (DataRow dd in ds.Tables[0].Rows)
                {
                    list.Add(DataRowToModel(dd));
                }
            }
            dr.Close();
            co.Close();
            return list;
        }

        public bool ExistsStu(int stuId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from T_Task_Choose");
            strSql.Append(" where StuId=@stuId");
            SqlParameter[] parameters = {
                    new SqlParameter("@stuId", SqlDbType.Int,4)
            };
            parameters[0].Value = stuId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

    }
}
