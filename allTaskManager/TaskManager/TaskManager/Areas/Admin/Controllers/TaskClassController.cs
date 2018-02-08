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
    public class TaskClassController : Controller
    {
        // GET: Admin/TaskClass
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

            DALT_Base_Class dal = new DALT_Base_Class();
            List<T_Base_Class> list = new List<T_Base_Class>();
            int pageSize = numPerPage;
            ViewBag.pageSize = numPerPage;
            //int pageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["pageSize"]);
            int pageIndex = pageNum;
            
            int recordcount = dal.GetRecord(where);
            if (!(keyword == ""))
            {
                where = "className like'%" + keyword + "%'";
            }
            list = dal.GetClassList(pageSize, pageIndex, where);

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

        public ActionResult GetAllTeacher()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Index");
            //}

            DALT_Base_Teacher dal = new DALT_Base_Teacher();
            List<T_Base_Teacher> lst = dal.GetAllList("1=1");

            string res = "[";

            foreach (T_Base_Teacher item in lst)
            {
                res += "{\"Name\":\"" + item.Name + "\",\"Id\":\"" + item.Id + "\"}";
                res += ",";
            }

            if (res.Count() >= 1)
                res = res.Substring(0, res.Count() - 1);

            res += "]";

            return Content(res);
        }

        public void AddSave(T_Base_Class cla)
        {
            DALT_Base_Class dal = new DALT_Base_Class();           
            cla.TeaId = Convert.ToInt32(Request.Form["Teacher.Id"]);
            bool res = dal.Add(cla);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"插入成功\",\"navTabId\":\"ClassList\",\"rel\":\"ClassList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"插入失败\",\"navTabId\":\"ClassList\",\"rel\":\"ClassList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public void Delete(string[] ids)
        {
            DALT_Base_Class dal = new DALT_Base_Class();

            string str = string.Join(",", ids);//将数组转化为string，中间用"，"隔开
            bool res = dal.DeleteList(str);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"删除成功\",\"navTabId\":\"ClassList\",\"rel\":\"ClassList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"删除失败\",\"navTabId\":\"ClassList\",\"rel\":\"ClassList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }

        public ActionResult ClassEdit(int id)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Login/Index");
            //}

            DALT_Base_Class db = new DALT_Base_Class();
            T_Base_Class cla = db.GetClass(id);

            ViewBag.item = cla;

            if (cla == null)
                return Content("资料不存在！");

            return View();
        }

        public void EditSave(T_Base_Class cla)
        {
            DALT_Base_Class db = new DALT_Base_Class();
            cla.TeaId = Convert.ToInt32(Request.Form["Teacher.Id"]);

            bool res = db.Update(cla);

            if (res)
            {
                string tmp = "{\"statusCode\":\"200\",\"message\":\"修改成功\",\"navTabId\":\"ClassList\",\"rel\":\"ClassList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"修改失败\",\"navTabId\":\"ClassList\",\"rel\":\"ClassList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
        }
    }
}