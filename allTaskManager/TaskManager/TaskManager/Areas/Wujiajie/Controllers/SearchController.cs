using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Model;
using TaskManager.DAL;

namespace TaskManager.Areas.Wujiajie.Controllers
{
    public class SearchController : Controller
    {
        // GET: Wujiajie/Search

        public ActionResult Index(string title = "", string start = "", int type = 0)
        {
            if(Session["UserId"] == null)
                return Redirect("/Login/Login/Index");

            int searchid = (int)Session["UserId"];
            int userLevel = (int)Session["UserLevel"];

            List<T_Search_Event> lst = new List<T_Search_Event>();
            if (type == 1 && (userLevel != 10 && userLevel != 11))
                lst = SearchMyTask(searchid, userLevel, title, start);
            else if (type == 2)
            {
                searchid = (int)Session["ClassId"];
                lst = SearchClassTask(searchid, userLevel, title, start);
            }
            else if(type == 3)
                lst = SearchCourseTask(searchid, userLevel, title, start);

            ViewBag.lst = lst;

            return View();
        }

        public List<T_Search_Event> SearchMyTask(int searchid, int userLevel,string title,string start)
        {
            string wtitel = "";
            string wstart = "";

            if (title != "")
                wtitel = " and Name like'%" + title + "%'";
            if(start != "")
            {
                wstart = " and convert(varchar(10),StartTime,120) = '" + start +"'";
            }

            string where = "StuId = " + searchid + wtitel + wstart;


            DALT_Event_MyTask dal = new DALT_Event_MyTask();
            List<T_Event_MyTask> lst = dal.GetAllList(where);
            List<T_Search_Event> list = new List<T_Search_Event>();

            #region 转换成现实格式
            foreach (T_Event_MyTask item in lst)
            {
                T_Search_Event now = new T_Search_Event();
                now.Name = item.Name;
                now.StartTime = (DateTime)item.StartTime;
                now.EndTime = (DateTime)item.EndTime;
                now.Description = item.Description;

                if (item.IsAlert == 1)
                    now.AlertTime = (int)item.AlertTime;
                else
                    now.AlertTime = 0;
                now.Type = item.Type;

                list.Add(now);
            }
            #endregion

            return list;
        }

        public List<T_Search_Event> SearchClassTask(int searchid, int userLevel, string title, string start)
        {
            string wtitel = "";
            string wstart = "";

            if (title != "")
                wtitel = " and Name like'%" + title + "%'";
            if (start != "")
            {
                wstart = " and convert(varchar(10),StartTime,120) = '" + start + "'";
            }

            string where = "ClassId = " + searchid + wtitel + wstart;

            DALT_Event_ClassTask dal = new DALT_Event_ClassTask();
            List<T_Event_ClassTask> lst = dal.GetAllList(where);
            List<T_Search_Event> list = new List<T_Search_Event>();

            #region 转换成现实格式
            foreach (T_Event_ClassTask item in lst)
            {
                T_Search_Event now = new T_Search_Event();
                now.Name = item.Name;
                now.StartTime = (DateTime)item.StartTime;
                now.EndTime = (DateTime)item.EndTime;
                now.Description = item.Description;

                if (item.IsAlert == 1)
                    now.AlertTime = (int)item.AlertTime;
                else
                    now.AlertTime = 0;
                now.Type = item.Type;

                list.Add(now);
            }
            #endregion

            return list;
        }

        public List<T_Search_Event> SearchCourseTask(int searchid, int userLevel, string title, string start)
        {
            string wtitel = "";
            string wstart = "";

            if (title != "")
                wtitel = " and Name like'%" + title + "%'";
            if (start != "")
            {
                wstart = " and convert(varchar(10),StartWeek,120) = '" + start +"'";
            }

            string where = "";

            DALT_Event_CourseTask dal = new DALT_Event_CourseTask();
            List<T_Search_Event> lst = new List<T_Search_Event>();

            if ( !(userLevel == 10 || userLevel == 11) )
            {
                where += "StuId = " + searchid + wtitel + wstart;
                lst = dal.GetAllSearch(0,where);
            }
            else
            {
                where += "TeaId = " + searchid + wtitel + wstart;
                lst = dal.GetAllSearch(1,where);
            }

            return lst;
        }
    }
}