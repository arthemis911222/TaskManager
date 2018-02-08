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
    public class CourseController : Controller
    {
      
        // GET: Wujiajie/Course
        public ActionResult Index3()
        {
           
            return View();
           
        }

        //王一情,并且把那个GetCourseTask放到了其他地方
        public int AddCourseTask(int type, string des, DateTime start, DateTime end, int taskalert, DateTime start_date, int week_type, int week_number, int userid, int choose_course)
        {
            T_Base_Teacher teacher = new T_Base_Teacher();
            DALT_Base_Teacher dal_teacher = new DALT_Base_Teacher();

            //王一情
            teacher = dal_teacher.GetModel(userid);

            T_Event_CourseTask model = new T_Event_CourseTask();
            model.Type = type;
            model.IsAlert = taskalert;
            model.CourseId = choose_course;
            model.Description = des;
            model.EndTime = end.ToLongTimeString().ToString();
            model.StartTime = start.ToLongTimeString().ToString();
            model.StartWeek = start_date;
            model.State = 0;
            model.WeekNum = week_number;
            model.WeekType = week_type;
            model.WPeople = teacher.Name;

            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();
            //循环添加课程日程

            int result = 0;
            if (model.WeekNum == 1)
            {
                result = dal.Add(model);
            }
            //如果有循环的话
            else if (model.WeekNum > 1)
            {
                for (int i = 1; i <= model.WeekNum; i++)
                {

                    //如果不分单双周
                    if (model.WeekType == 0)
                    {
                        result = dal.Add(model);
                        model.StartWeek = start_date.AddDays(7 * i);
                    }
                    //如果单周
                    else if (model.WeekType == 1)
                    {
                        result = dal.Add(model);
                        model.StartWeek = start_date.AddDays(14 * i);
                    }
                    //如果双周
                    else if (model.WeekType == 2)
                    {
                        model.StartWeek = start_date.AddDays(14 * (i - 1) + 7);
                        result = dal.Add(model);
                    }
                }
            }
            if (result > 0)
            {
                return 2;
            }
            return 0;
        }


        //王一情
        public int EditCourseTask(string title, int type, string des, DateTime start, DateTime end, int alert, int userid, int taskid)
        {
            if (title == "")
            {
                return 1;
            }
            else if (userid == -1)
            {
                return 2;
            }

            #region 课程日程修改
            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();
            T_Event_CourseTask item = new T_Event_CourseTask();
            T_Event_CourseTask course_task = new T_Event_CourseTask();
            course_task = dal.GetModel(taskid);

            item.Id = taskid;
            item.State = 0;
            item.Type = type;
            item.Description = des;
            item.StartTime = start.ToLongTimeString().ToString();
            item.EndTime = end.ToLongTimeString().ToString();
            item.WPeople = course_task.WPeople;
            item.CourseId = course_task.CourseId;
            item.StartWeek = start;
            item.WeekNum = 1;

            if (alert == 0)
            {
                item.IsAlert = 0;
            }
            else
            {
                item.IsAlert = 1;
                item.IsAlert = alert;
            }

            #endregion

            bool res = dal.Update(item);

            if (res)
                return 0;
            else
                return 3;
        }


        //王一情 
        //冲突检测（自己和自己不存在冲突——吴佳洁）
        public int IsCouChongtu(DateTime start, DateTime end, DateTime start_date, int userid, int week_type, int week_number)
        {
            //王一情
            int re = 1;
            DateTime date = start_date;

            //如果有循环的话
            if (week_number > 1)
            {
                for (int i = 1; i <= week_number; i++)
                {
                    //如果不分单双周
                    if (week_type == 0)
                    {
                        date = start_date.AddDays(7 * (i - 1));
                        re = GetAllTime(start, end, date, userid, week_type, week_number);
                    }
                    //如果单周
                    else if (week_type == 1)
                    {

                        date = start_date.AddDays(14 * (i - 1));
                        re = GetAllTime(start, end, date, userid, week_type, week_number);
                    }
                    //如果双周
                    else if (week_type == 2)
                    {
                        date = start_date.AddDays(14 * (i - 1) + 7);
                        re = GetAllTime(start, end, date, userid, week_type, week_number);
                    }
                }
            }
            return re;

        }

        public int GetAllTime(DateTime start, DateTime end, DateTime start_date, int userid, int week_type, int week_number)
        {

            #region 判断课程和课程冲突
            //王一情 需要先找到这个老师所教的所有课程，然后进行冲突判断
            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();
            List<T_Event_CourseTask> course_list = dal.GetTeaCourseList2(userid);

            DateTime st = start_date;
            string ym = st.ToString("yyyy-MM-dd");
            string strSt = ym + " " + start.ToLongTimeString().ToString();
            string strEn = ym + " " + end.ToLongTimeString().ToString();
            DateTime newSt = Convert.ToDateTime(strSt);
            DateTime newEn = Convert.ToDateTime(strEn);

            foreach (T_Event_CourseTask item in course_list)
            {
                if (item.State == 1)
                    continue;

                DateTime st2 = (DateTime)item.StartWeek;
                string om = st2.ToString("yyyy-MM-dd");
                string strSt2 = om + " " + Convert.ToDateTime(item.StartTime).ToLongTimeString().ToString();
                string strEn2 = om + " " + Convert.ToDateTime(item.EndTime).ToLongTimeString().ToString();
                DateTime oldSt = Convert.ToDateTime(strSt2);
                DateTime oldEn = Convert.ToDateTime(strEn2);

                if (!(newSt >= oldEn || newEn <= oldSt))
                {
                    return 0;
                }
            }
            #endregion

            #region 判断课程和班级冲突
            //王一情
            //需要先通过courseid找到选这个课的学生的班级，然后查找班级日程
            DALT_Event_ClassTask classdal = new DALT_Event_ClassTask();
            List<T_Event_ClassTask> classList = classdal.GetTeaClassList2(userid);

            foreach (T_Event_ClassTask item in classList)
            {
                if (item.State == 1)
                    continue;

                if (!(newSt >= item.EndTime || newEn <= item.StartTime))
                {
                    return 0;
                }
            }
            #endregion

            return 1;
        }

        //王一情 
        //冲突检测（自己和自己不存在冲突——吴佳洁）
        public int IsCouChongtu2(DateTime start, DateTime end, int taskid, int userid)
        {

            #region 判断课程和课程冲突
            //王一情 需要先找到这个老师所教的所有课程，然后进行冲突判断
            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();
            List<T_Event_CourseTask> course_list = dal.GetTeaCourseList2(userid);

            foreach (T_Event_CourseTask item in course_list)
            {
                //王一情
                if (item.State == 1 || taskid == item.Id)
                    continue;

                DateTime st2 = (DateTime)item.StartWeek;
                string om = st2.ToString("yyyy-MM-dd");
                string strSt2 = om + " " + Convert.ToDateTime(item.StartTime).ToLongTimeString().ToString();
                string strEn2 = om + " " + Convert.ToDateTime(item.EndTime).ToLongTimeString().ToString();
                DateTime oldSt = Convert.ToDateTime(strSt2);
                DateTime oldEn = Convert.ToDateTime(strEn2);

                if (!(start >= oldEn || end <= oldSt))
                {
                    return 0;
                }
            }
            #endregion

            #region 判断课程和班级冲突
            //王一情
            //需要先通过courseid找到选这个课的学生的班级，然后查找班级日程
            DALT_Event_ClassTask classdal = new DALT_Event_ClassTask();
            List<T_Event_ClassTask> classList = classdal.GetTeaClassList2(userid);

            foreach (T_Event_ClassTask item in classList)
            {
                if (item.State == 1)
                    continue;

                if (!(start >= item.EndTime || end <= item.StartTime))
                {
                    return 0;
                }
            }
            #endregion

            return 1;
        }

        //王一情
        public string GetOneCourseTask(int id)
        {
            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();
            T_Event_CourseTask item = dal.GetModel(id);

            //DALT_Task_Course dal_cou = new DALT_Task_Course();
            //T_Task_Course course = new T_Task_Course();
            //course = dal_cou.GetModel(courseId);

            #region 连接传回数据
            string res = "[";
            int taskAlert = 0;

            if (item.IsAlert != 0)
            {
                taskAlert = (int)item.IsAlert;  //是否提醒
            }

            res += "{\"type\":\"" + item.Type
                + "\",\"des\":\"" + item.Description
                + "\",\"taskalert\":\"" + taskAlert
                 + "\",\"title\":\"" + "shu" + "\"}";
            res += ",";

            if (res.Count() >= 1)
                res = res.Substring(0, res.Count() - 1);

            res += "]";
            #endregion


            return res;
        }


        //王一情
        public int DeleteCourseTask(int taskid)
        {

            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();

            dal.Delete(taskid);

            return 1;
        }
    }
}