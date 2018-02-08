using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Model;
using TaskManager.DAL;
using System.Text;

namespace TaskManager.Areas.Wujiajie.Controllers
{
    public class StudentController : Controller
    {
        // GET: Wujiajie/Student
        public ActionResult Index()
        {
            return View();
        }

        //冲突检测
        public int IsChongtu(string start, string end)
        {
            DateTime dt_start = Convert.ToDateTime(start);
            DateTime dt_end = Convert.ToDateTime(end);

            #region 判断班级冲突

            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();
            List<T_Event_ClassTask> class_list = dal.GetAllList();

            foreach (T_Event_ClassTask item in class_list)
            {
                if (item.State == 1)
                    continue;

                if (!(dt_start > (DateTime)item.EndTime || dt_end < (DateTime)item.StartTime))
                {
                    return 0;
                }
            }
            #endregion

            #region 判断课程冲突
            DALT_Event_CourseTask coursedal = new DALT_Event_CourseTask();
            List<T_Event_CourseTask> course_list = coursedal.GetAllList();

            foreach (T_Event_CourseTask item in course_list)
            {
                if (item.State == 1)
                    continue;

                DateTime st = (DateTime)item.StartWeek;
                string ym = st.ToString("yyyy-MM-dd");
                string strSt = ym + " " + item.StartTime;
                string strEn = ym + " " + item.EndTime;
                DateTime newSt = Convert.ToDateTime(strSt);
                DateTime newEn = Convert.ToDateTime(strEn);

                if (!(dt_start >= newEn || dt_end <= newSt))
                {
                    return 0;
                }
            }
            #endregion

            return 1;
        }

        //新建我的日程
        public int AddMyTask(string title,int type,string des,string start,string end,int alert,int userid = -1,int state=0)
        {
            if(title == "")
            {
                return 1;
            }else if(userid == -1)
            {
                return 2;
            }

            #region 我的日程创建
            DALT_Event_MyTask dal = new DALT_Event_MyTask();
            T_Event_MyTask item = new T_Event_MyTask();
            item.State = state;
            item.Name = title;
            item.Type = type;
            item.Description = des;
            item.StuId = userid;
            item.StartTime = Convert.ToDateTime(start);
            item.EndTime = Convert.ToDateTime(end);

            if(alert == 0)
            {
                item.IsAlert = 0;
            }else
            {
                item.IsAlert = 1;
                item.AlertTime = alert;
            }

            int res = dal.Add(item);

            #endregion

            if (res != 0)
                return 0;
            else
                return 3;
        }

        //修改我的日程
        public int EditMyTask(string title, int type, string des, string start, string end, int alert,int taskid,int userid=-1)
        {
            if (title == "")
            {
                return 1;
            }
            else if (userid == -1)
            {
                return 2;
            }

            #region 我的日程修改
            DALT_Event_MyTask dal = new DALT_Event_MyTask();
            T_Event_MyTask item = new T_Event_MyTask();
            item.Id = taskid;
            item.StuId = userid;//学生Id,之后修改
            item.Name = title;
            item.Type = type;
            item.Description = des;
            item.StartTime = Convert.ToDateTime(start);
            item.EndTime = Convert.ToDateTime(end);

            if (alert == 0)
            {
                item.IsAlert = 0;
            }
            else
            {
                item.IsAlert = 1;
                item.AlertTime = alert;
            }

            bool res = dal.Update(item);

            #endregion

            if (res)
                return 0;
            else
                return 3;
        }

        //删除我的日程
        public int DeleteMyTask(int taskid)
        {
            bool res;

            DALT_Event_MyTask dal = new DALT_Event_MyTask();
            res = dal.Delete(taskid);

            if (res)
                return 1;
            else
                return 0;
        }

        public string GetOneMyTask(int id)
        {
            DALT_Event_MyTask dal = new DALT_Event_MyTask();
            T_Event_MyTask item = dal.GetModel(id);

            string res = "[";

            int taskAlert = 0;

            if(item.IsAlert != 0)
            {
                taskAlert = (int)item.AlertTime;
            }

            res += "{\"type\":\"" + item.Type
                + "\",\"des\":\"" + item.Description
                + "\",\"taskAlert\":\"" + taskAlert
                + "\",\"userid\":\"" + item.StuId + "\"}";
            res += ",";

            if (res.Count() >= 1)
                res = res.Substring(0, res.Count() - 1);

            res += "]";

            return res;
        }

    }
}