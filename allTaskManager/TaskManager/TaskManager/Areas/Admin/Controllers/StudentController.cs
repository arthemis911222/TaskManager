using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.BLL;
using TaskManager.DAL;
using TaskManager.Model;

namespace TaskManager.Areas.Admin.Controllers
{
    public class StudentController : Controller
    {
        // GET: Admin/Student
        public ActionResult Index(string keyword, int pageNum = 1, int numPerPage = 10)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Login/Index");
            //}

            string where = "1=1";

            if (!(keyword == ""))
            {
                where = "Name like'%" + keyword + "%'";
            }

            DALT_Base_Student dal = new DALT_Base_Student();
            List<T_Base_Student> list = new List<T_Base_Student>();
            int pageSize = numPerPage;
            ViewBag.pageSize = numPerPage;
            //int pageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["pageSize"]);
            int pageIndex = pageNum;
            int recordcount = dal.GetRecord(where);
            list = dal.GetStudentList(pageSize, pageIndex, where);

            ViewBag.lst = list;
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.recordCount = recordcount;

            return View();
        }

        public ActionResult Add()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Login/Index");
            //}
            return View();
        }

        public ActionResult GetAllClass()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Index");
            //}

            DALT_Base_Class dal = new DALT_Base_Class();
            List<T_Base_Class> lst = dal.GetAllList("1=1");

            string res = "[";

            foreach (T_Base_Class item in lst)
            {
                res += "{\"Name\":\"" + item.Name + "\",\"Id\":\"" + item.Id + "\"}";
                res += ",";
            }

            if (res.Count() >= 1)
                res = res.Substring(0, res.Count() - 1);

            res += "]";

            return Content(res);
        }

        public void AddSave(T_Base_Student student)
        {
            DALT_Base_Student dal = new DALT_Base_Student();
            string pwd = MD5Class.UserMd5(student.StuId);
            student.ClassId= Convert.ToInt32(Request.Form["Class.Id"]);
            student.PassWord = pwd;
            int res = dal.Add(student);

            if (res != 0)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"插入成功\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"插入失败\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public void Delete(string[] ids)
        {
            DALT_Base_Student dal = new DALT_Base_Student();
            DAL.DALT_Event_CourseTask dal2 = new DALT_Event_CourseTask();
            DAL.DALT_ClassTask_Stu dal3 = new DALT_ClassTask_Stu();
            string str = string.Join(",", ids);//将数组转化为string，中间用"，"隔开
            bool res = dal.DeleteList(str);
            //int[] ids2 = new int[ids.Length];
            /*for(int i=0;i<ids.Length;i++)   //wx
            {
                ids2[i] = Integer.parseInt(ids[i]);
            }
            for (int i=0;i<ids.Length;i++)
            {
                if(dal2.ExistsStu(ids[i])==true)
            }*/
            
            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"删除成功\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"删除失败\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public ActionResult StudentEdit(int id)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Login/Index");
            //}

            DALT_Base_Student db = new DALT_Base_Student();
            T_Base_Student student = db.GetStudent(id);

            ViewBag.item = student;

            if (student == null)
                return Content("资料不存在！");

            return View();
        }

        public void EditSave(T_Base_Student student)
        {
            DALT_Base_Student db = new DALT_Base_Student();
            T_Base_Student student2 = db.GetStudent(student.Id);
            student.PassWord = student2.PassWord;
            student.ClassId = Convert.ToInt32(Request.Form["Class.Id"]);

            bool res = db.Update(student);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"修改成功\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"修改失败\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public void Reset(int id)
        {
            DALT_Base_Student db = new DALT_Base_Student();
            T_Base_Student student = db.GetStudent(id);
            string pwd = MD5Class.UserMd5(student.StuId);
            student.PassWord = pwd;
            bool res = db.Update(student);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"重置密码成功\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"重置密码失败\",\"navTabId\":\"StudentList\",\"rel\":\"StudentList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }
        //public void OutData()
        //{
        //    DALT_Base_Student dal = new DALT_Base_Student();
        //    DataTable dt = dal.GetList("1=1").Tables[0];

        //    MyNPOI.ExportExcel(dt, "");
        //}
    }
}
