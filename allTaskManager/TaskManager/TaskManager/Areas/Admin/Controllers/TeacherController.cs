using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.BLL;
using TaskManager.DAL;
using TaskManager.Model;

namespace TaskManager.Areas.Admin.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Admin/Teacher
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

            DALT_Base_Teacher dal = new DALT_Base_Teacher();
            List<T_Base_Teacher> list = new List<T_Base_Teacher>();
            int pageSize = numPerPage;
            ViewBag.pageSize = numPerPage;
            //int pageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["pageSize"]);
            int pageIndex = pageNum;
            int recordcount = dal.GetRecord(where);
            list = dal.GetTeacherList(pageSize, pageIndex, where);

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

        public void AddSave(T_Base_Teacher teacher)
        {
            DALT_Base_Teacher dal = new DALT_Base_Teacher();
            string pwd = MD5Class.UserMd5(teacher.TeaId);           
            teacher.PassWord = pwd;
            bool res = dal.Add(teacher);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"插入成功\",\"navTabId\":\"TeacherList\",\"rel\":\"TeacherList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"插入失败\",\"navTabId\":\"TeacherList\",\"rel\":\"TeacherList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public void Delete(string[] ids)
        {
            DALT_Base_Teacher dal = new DALT_Base_Teacher();

            string str = string.Join(",", ids);//将数组转化为string，中间用"，"隔开
            bool res = dal.DeleteList(str);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"删除成功\",\"navTabId\":\"TeacherList\",\"rel\":\"TeacherList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"删除失败\",\"navTabId\":\"TeacherList\",\"rel\":\"TeacherList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public ActionResult TeacherEdit(int id)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Login/Index");
            //}

            DALT_Base_Teacher db = new DALT_Base_Teacher();
            T_Base_Teacher teacher = db.GetTeacher(id);

            ViewBag.item = teacher;

            if (teacher == null)
                return Content("资料不存在！");

            return View();
        }

        public void EditSave(T_Base_Teacher teacher)
        {
            DALT_Base_Teacher db = new DALT_Base_Teacher();
            T_Base_Teacher teacher2 = db.GetTeacher(teacher.Id);
            teacher.PassWord = teacher2.PassWord;

            bool res = db.Update(teacher);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"修改成功\",\"navTabId\":\"TeacherList\",\"rel\":\"TeacherList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"修改失败\",\"navTabId\":\"TeacherList\",\"rel\":\"TeacherList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public void Reset(int id)
        {
            DALT_Base_Teacher db = new DALT_Base_Teacher();
            T_Base_Teacher teacher = db.GetTeacher(id);
            string pwd = MD5Class.UserMd5(teacher.TeaId);
            teacher.PassWord = pwd;
            bool res = db.Update(teacher);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"重置密码成功\",\"navTabId\":\"teacherList\",\"rel\":\"teacherList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"重置密码失败\",\"navTabId\":\"teacherList\",\"rel\":\"teacherList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }
    }
}