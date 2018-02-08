using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;
using TaskManager.Model;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //获取所有日程（显示日历）
        public string GetAllTask(int userlevel,int userid,int classid)
        {

            string res1 = "";
            string res2 = "";
            string res3 = "";

            if (userlevel == 0 || userlevel == 1 || userlevel == 2 || userlevel == 3)
            {
                res1 = GetMyTask(userid);  //学生的
                res2 = GetClassTask(classid);  //班级的

                //先通过学生的ID找到所选的课程号，然后找到这个课程的课程日程
                res3 = GetCourseTask(userid, userlevel);   //课程的
            }
            else if (userlevel == 10)  //普通教师
            {
                res3 = GetCourseTask(userid, userlevel);   //课程的
            }
            else if (userlevel == 11)
            {
                res2 = GetClassTask(classid);  //班级的
                res3 = GetCourseTask(userid, userlevel);   //课程的
            }

            string res = "[";

            res += res1 + res2 + res3;

            if (res.Count() >= 1)
                res = res.Substring(0, res.Count() - 1);

            res += "]";

            if (res == "]")
                res = "[]";

            return res;
        }

        //-记得修改stuid
        public string GetMyTask(int stuId)
        {

            //int stuId = (int)Session["userId"];
            //int stuId = 4;
            string where = "StuId = " + stuId;

            DALT_Event_MyTask dal = new DALT_Event_MyTask();
            List<T_Event_MyTask> list = new List<T_Event_MyTask>();
            list = dal.GetAllList(where);//传参数where

            string res = "";

            foreach (T_Event_MyTask item in list)
            {
                if (item.State == 1)
                    continue;

                string st = ((DateTime)item.StartTime).ToString("yyyy-MM-dd HH:mm");
                string ed = ((DateTime)item.EndTime).ToString("yyyy-MM-dd HH:mm");
                res += "{\"id\":\"" + item.Id
                    + "\",\"title\":\"" + item.Name
                    + "\",\"start\":\"" + st
                    + "\",\"type\":\"" + item.Type
                    + "\",\"end\":\"" + ed + "\"}";
                res += ",";
            }

            return res;

        }

        //-记得修改classid
        public string GetClassTask(int classId)
        {

            //int classId = (int)Session["classid"];
            //int classId = 2;
            string where = "ClassId = " + classId;

            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();
            List<T_Event_ClassTask> list = new List<T_Event_ClassTask>();
            list = dal.GetAllList(where);//传参数where

            string res = "";

            foreach (T_Event_ClassTask item in list)
            {
                if (item.State == 1)
                    continue;
                string st = ((DateTime)item.StartTime).ToString("yyyy-MM-dd HH:mm");
                string ed = ((DateTime)item.EndTime).ToString("yyyy-MM-dd HH:mm");
                res += "{\"id\":\"" + item.Id
                    + "\",\"title\":\"" + item.Name
                    + "\",\"start\":\"" + st
                    + "\",\"type\":\"" + item.Type
                    + "\",\"end\":\"" + ed + "\"}";
                res += ",";
            }

            return res;

        }

        //王一情
        public string GetCourseTask(int userid, int userlevel)
        {
            T_Base_Teacher teacher = new T_Base_Teacher();
            DALT_Base_Teacher dal_Teacher = new DALT_Base_Teacher();
            teacher = dal_Teacher.GetModel(userid);

            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();
            List<T_Event_CourseTask> list = new List<T_Event_CourseTask>();
            if (userlevel != 10 && userlevel != 11)
            {
                list = dal.GetStuCourseList(userid);
            }
            else
            {
                list = dal.GetTeaCourseList("WPeople=" + "'" + teacher.Name + "'");
            }

            string res = "";

            foreach (T_Event_CourseTask item in list)
            {
                T_Task_Course course = new T_Task_Course();
                DALT_Task_Course dal_course = new DALT_Task_Course();
                course = dal_course.GetModel(item.CourseId);

                string st_date = ((DateTime)item.StartWeek).ToString("yyyy-MM-dd");
                string st = st_date + " " + item.StartTime;
                string ed = st_date + " " + item.EndTime;

                res += "{\"id\":\"" + item.Id
                    + "\",\"title\":\"" + course.Name
                    + "\",\"type\":\"" + item.Type
                    + "\",\"des\":\"" + item.Description
                    + "\",\"start\":\"" + st
                    + "\",\"end\":\"" + ed + "\"}";
                res += ",";
            }


            return res;

        }
    }
}