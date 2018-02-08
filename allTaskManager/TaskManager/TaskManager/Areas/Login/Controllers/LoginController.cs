using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.BLL;
using TaskManager.DAL;
using TaskManager.Model;

namespace TaskManager.Areas.Login.Controllers
{
    public class LoginController : Controller
    {
        //public ActionResult Index()
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        Response.Redirect("/home/Login");
        //        return null;
        //    }

        //    return View();
        //}

        // GET: Login/Login
        public ActionResult Index()
        {          
            return View();
        }

        public ActionResult TetS()
        {
            if (Session["UserId"] == null)
                return Redirect("/Login/Login/Index");

            List<T_Task_Course> list = FindAllCourse(1);

            DateTime d = DateTime.Now;                                         //wx
            int id = (int)Session["UserId"];
            DAL.DALT_Event_MyTask dal = new DALT_Event_MyTask();
            DAL.DALT_Event_ClassTask dal2 = new DALT_Event_ClassTask();
            DAL.DALT_Event_CourseTask dal3 = new DALT_Event_CourseTask();
            List<T_Event_MyTask> list2 = dal.FindList(d, id);
            List<T_Event_ClassTask> list3 = dal2.FindList(d, id);
            List<T_Event_CourseTask> list4 = dal3.FindList(d, id);

            ViewBag.list = list;
            ViewBag.list2 = list2;
            ViewBag.list3 = list3;
            ViewBag.list4 = list4;

            return View();
        }

        public ActionResult TetT()
        {
            if (Session["UserId"] == null)
                return Redirect("/Login/Login/Index");

            int Teaid = (int)Session["UserId"];

            List<T_Task_Course> list = FindAllCourse(Teaid);

            DateTime d = DateTime.Now;                                         //wx
            int id = (int)Session["UserId"];
            DAL.DALT_Event_MyTask dal = new DALT_Event_MyTask();
            DAL.DALT_Event_ClassTask dal2 = new DALT_Event_ClassTask();
            DAL.DALT_Event_CourseTask dal3 = new DALT_Event_CourseTask();
            List<T_Event_ClassTask> list3 = dal2.FindListT(d, id);
            List<T_Event_CourseTask> list4 = dal3.FindListT(d, id);
            List<T_Event_MyTask> list2 = dal.FindList(d, id);
            ViewBag.list2 = list2;
            ViewBag.list3 = list3;
            ViewBag.list4 = list4;

            ViewBag.list = list;
            return View();
        }

        //找到登陆的用户所管理的课程
        public List<T_Task_Course> FindAllCourse(int Teaid)
        {
            //找到登陆的用户所管理的课程
            DALT_Task_Course dal_course = new DALT_Task_Course();
            List<T_Task_Course> list = dal_course.FindCourse(Teaid);
            //  List<T_Task_Course> list = dal_course.GetList();
            var ab = list.Count();

            return list;
        }

        public JsonResult LoginCheck(string Name, string password, string checkres)
        {
            string pwd = MD5Class.UserMd5(password);
            if (checkres == "学生")
            {
                DAL.DALT_Base_Student dal = new DAL.DALT_Base_Student();
                Model.T_Base_Student student = dal.GetStu(Name);
                if (ifExist(Name, pwd, checkres))
                {
                    Session["UserName"] = student.Name;
                    Session["UserId"] = student.Id;
                    Session["ClassId"] = student.ClassId;

                    Session["UserLevel"] = 0;
                    if (student.IsBGB == 1 && student.IsKDB == 1)
                        Session["UserLevel"] = 3;
                    else if (student.IsBGB == 1)
                        Session["UserLevel"] = 2;
                    else if (student.IsKDB == 1)
                        Session["UserLevel"] = 1;
                    

                    return Json(new { code = 11, message = "登录成功" }, JsonRequestBehavior.AllowGet);
                }
               
                else
                {
                    return Json(new { code = 3, message = "用户名或密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (checkres == "老师")
            {
                DAL.DALT_Base_Teacher dal = new DAL.DALT_Base_Teacher();
                Model.T_Base_Teacher teacher = dal.GetTea(Name);
                DAL.DALT_Base_Class dal2 = new DAL.DALT_Base_Class();
                Model.T_Base_Class cla = new T_Base_Class();

                if (teacher.IsBZR == 1)
                {
                    cla = dal2.FindCla(teacher.Id);
                    Session["ClassId"] = cla.Id;
                }
                else
                    Session["ClassId"] = 0;

                if (ifExist(Name, pwd, checkres))
                {
                    Session["UserName"] = teacher.Name;
                    Session["UserId"] = teacher.Id;

                    Session["UserLevel"] = 10;
                    if (teacher.IsBZR == 1)
                        Session["UserLevel"] = 11;

                    return Json(new { code = 12, message = "登录成功" }, JsonRequestBehavior.AllowGet);

                }
                
                else
                {
                    return Json(new { code = 3, message = "用户名或密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                DAL.DALT_Base_Admin dal = new DAL.DALT_Base_Admin();
                Model.T_Base_Admin admin = dal.GetAdmin(Name);
                if (ifExist(Name, pwd, checkres))
                {
                    Session["UserId"] = admin.Id;
                    Session["Name"] = admin.LoginName;
                    return Json(new { code = 13, message = "登录成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 3, message = "用户名或密码错误" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public bool ifExist(string Name, string password, string checkres)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["sqlconnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();

            if(checkres=="学生")
            cm.CommandText = "select * from t_base_student where StuId=@userName and password=@password;";
            else if(checkres=="老师")
                cm.CommandText = "select * from t_base_teacher where TeaId=@userName and password=@password;";
            else
                cm.CommandText = "select * from t_base_admin where LoginName=@userName and password=@password;";
            cm.Parameters.AddWithValue("@userName", Name);
            cm.Parameters.AddWithValue("@passWord", password);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            int count = 0;
            while (dr.Read())
            {
                //有大于零条记录存在即为验证成功
                count = 1;

            }
            if (count == 1)
            {
                dr.Close();
                co.Close();
                return true;
            }
            else
            {
                dr.Close();
                co.Close();
                return false;
            }
        }

        //搜索界面返回首页
        public ActionResult GoHome()
        {
            if (Session["UserId"] == null)
                return Redirect("/Login/Login/Index");

            int level = (int)Session["UserLevel"];

            if(level == 10 || level == 11)
                return Redirect("/Login/Login/TetT");
            else
                return Redirect("/Login/Login/TetS");
        }

    }
}