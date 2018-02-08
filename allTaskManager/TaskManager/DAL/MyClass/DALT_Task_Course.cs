using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TaskManager.Model;

namespace TaskManager.DAL
{
    public partial class DALT_Task_Course
    {
        public List<T_Task_Course> GetList()
        {

            //连接C:\Users\lenovo\Desktop\taskmanager\TaskManager\DAL\MyClass\DALT_Event_MyTask.cs
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            //读取
            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from T_Task_Course;";
            //发送
            cm.Connection = co;

            //接收
            SqlDataReader dr = cm.ExecuteReader();
            List<TaskManager.Model.T_Task_Course> lst = new List<TaskManager.Model.T_Task_Course>();

            while (dr.Read())
            {
                TaskManager.Model.T_Task_Course course = new TaskManager.Model.T_Task_Course();
                course.ClassId = Convert.ToInt32(dr["ClassId"]);
                course.CourseId = Convert.ToString(dr["CourseId"]);
                course.Id = Convert.ToInt32(dr["Id"]);
                course.Name = Convert.ToString(dr["Name"]);
                course.StuId = Convert.ToInt32(dr["StuId"]);
                course.TeaId = Convert.ToInt32(dr["TeaId"]);
                course.Type = Convert.ToInt32(dr["Type"]);
                lst.Add(course);
            }
            dr.Close();
            co.Close();
            return lst;

        }

        //根据TeachId查找CourseName
        public List<T_Task_Course> FindCourse(int id)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select Id from T_Task_Course where TeaId=" + id;

            co.Open();
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            List<T_Task_Course> list = new List<T_Task_Course>();
            while (dr.Read())
            {
                T_Task_Course head = new T_Task_Course();
                DALT_Task_Course head_dal = new DALT_Task_Course();
                int Id = Convert.ToInt32(dr["Id"]);
                head = head_dal.GetModel(Id);
                list.Add(head);
            }
            dr.Close();
            co.Close();
            return list;
        }

    }
}
