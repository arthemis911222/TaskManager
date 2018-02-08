using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Model;
using TaskManager.DAL;

namespace TaskManager.Areas.Wujiajie.Controllers
{
    public class ClassTaskController : Controller
    {
        // GET: Wujiajie/ClassTask
        public ActionResult Index()
        {
            return View();
        }

        //-班级和课程冲突判定
        public int IsChongtu(int taskid,string start,string end)
        {
            DateTime dt_start = Convert.ToDateTime(start);
            DateTime dt_end = Convert.ToDateTime(end);

            #region 判断班级和班级冲突

            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();
            List<T_Event_ClassTask> class_list = dal.GetAllList();

            foreach(T_Event_ClassTask item in class_list)
            {
                if (taskid == item.Id || item.State == 1)
                    continue;

                if(!(dt_start >= (DateTime)item.EndTime || dt_end <= (DateTime)item.StartTime))
                {
                    return 0;
                }
            }
            #endregion

            #region 判断班级和课程冲突
            DALT_Event_CourseTask coursedal = new DALT_Event_CourseTask();
            List<T_Event_CourseTask> course_list = coursedal.GetAllList();

            foreach (T_Event_CourseTask item in course_list)
            {
                if (item.State == 1)
                    continue;

                DateTime st = (DateTime)item.StartWeek;
                string ym = st.ToString("yyyy-MM-dd");
                string strSt = ym +  " " + item.StartTime;
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

        public string GetAllStu(int classid)
        {
            DALT_Base_Student dal = new DALT_Base_Student();
            List<T_Base_Student> list = new List<T_Base_Student>();
            list = dal.GetAllStudents(classid);

            string res = "";

            foreach (T_Base_Student item in list)
            {
                res += item.Id + "," + item.Name + ",";
            }

            res = res.Substring(0, res.LastIndexOf(','));

            return res;
        }

        //班干部 and 老师
        //班级任务添加-记得修改stuid
        public int AddClassTask(string title, int type, string des, string start, string end, 
            int alert,int isAll,string students,string WPeople,int userLevel, int classid=-1,int userid=-1,int state=0)
        {

            if (title == "")
            {
                return 1;
            }
            else if (classid == -1)
            {
                return 2;
            }

            #region 班级日程创建
            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();
            T_Event_ClassTask item = new T_Event_ClassTask();
            item.State = state;
            item.Name = title;
            item.Type = type;
            item.Description = des;
            item.ClassId = classid;
            item.StartTime = Convert.ToDateTime(start);
            item.EndTime = Convert.ToDateTime(end);
            item.WPeople = WPeople;
            item.IsAllStuTask = isAll;

            if (alert == 0)
            {
                item.IsAlert = 0;
            }
            else
            {
                item.IsAlert = 1;
                item.AlertTime = alert;
            }

            #endregion

            int res = 0;
            if (userLevel == 0)
                res = dal.AddTaskStu(item, userid);//res也是taskid
            else
                res = dal.AddTaskTea(item, userid);

            #region 部分学生的班级日程添加
            if (isAll == 0)
            {
                students = students.Substring(0, students.LastIndexOf(","));
                string[] stus = students.Split(',');

                DALT_Event_StuClassTask escDal = new DALT_Event_StuClassTask();
                escDal.AddPartTask(res, stus);

            }
            #endregion

            if (res != 0)
                return 0;
            else
                return 3;
        }

        //班级任务修改
        public int EditClassTask(int taskid,string title, int type, string des, string start,
            string end, int alert,int isAll,string students,string WPeople,int classid=-1)
        {

            if (title == "")
            {
                return 1;
            }
            else if (classid == -1)
            {
                return 2;
            }

            #region 班级日程修改
            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();
            T_Event_ClassTask item = new T_Event_ClassTask();
            item.Id = taskid;
            item.State = 0;
            item.Name = title;
            item.Type = type;
            item.Description = des;
            item.ClassId = classid;
            item.StartTime = Convert.ToDateTime(start);
            item.EndTime = Convert.ToDateTime(end);
            item.WPeople = WPeople;
            item.IsAllStuTask = isAll;

            if (alert == 0)
            {
                item.IsAlert = 0;
            }
            else
            {
                item.IsAlert = 1;
                item.AlertTime = alert;
            }

            #endregion

            bool res = dal.Update(item);

            #region 删除原有的，重新添加
            if (isAll == 0)
            {
                students = students.Substring(0, students.LastIndexOf(","));
                string[] stus = students.Split(',');

                DALT_Event_StuClassTask escDal = new DALT_Event_StuClassTask();
                escDal.EditPartTask(taskid, stus);
            }
            #endregion

            if (res)
                return 0;
            else
                return 3;
        }

        //班级任务删除
        public int DeleteClassTask(int userLevel, int taskid)
        {
            string where = "ClassId = " + taskid;
            if (userLevel == 0)
            {
                DALT_ClassTask_Stu csDal = new DALT_ClassTask_Stu();
                csDal.DeleteWhere(where);
            }else
            {
                DALT_ClassTask_Tea csDal = new DALT_ClassTask_Tea();
                csDal.DeleteWhere(where);
            }

            DALT_Event_ClassTask ectDal = new DALT_Event_ClassTask();

            T_Event_ClassTask item = new T_Event_ClassTask();
            item = ectDal.GetModel(taskid);

            if (item.IsAllStuTask == 0)
            {
                DALT_Event_StuClassTask escDal = new DALT_Event_StuClassTask();

                where = "ClassTaskId = " + taskid;
                escDal.DeleteWhere(where);
            }

            ectDal.Delete(taskid);

            return 1;
        }

        public string GetOneClassTask(int userLevel,int taskid,int userid,int classid)
        {
            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();
            T_Event_ClassTask item = dal.GetModel(taskid);

            int canEdit = dal.CanEdit(userLevel,userid, taskid);
            string allstudents = GetAllStu(classid);
            

            #region 连接传回数据
            string res = "[";

            int taskAlert = 0;

            if (item.IsAlert != 0)
            {
                taskAlert = (int)item.AlertTime;
            }

            string students = "";
            if(item.IsAllStuTask != 1)
            {
                //部分学生
                DALT_Event_StuClassTask escDal = new DALT_Event_StuClassTask();
                students = escDal.GetStus(taskid);

            }

            res += "{\"type\":\"" + item.Type
                + "\",\"des\":\"" + item.Description
                + "\",\"taskAlert\":\"" + taskAlert
                + "\",\"students\":\"" + students
                + "\",\"canEdit\":\"" + canEdit
                + "\",\"allstudents\":\"" + allstudents
                + "\",\"isAll\":\"" + item.IsAllStuTask + "\"}";
            res += ",";

            if (res.Count() >= 1)
                res = res.Substring(0, res.Count() - 1);

            res += "]";
            #endregion


            return res;
        }


    }
}