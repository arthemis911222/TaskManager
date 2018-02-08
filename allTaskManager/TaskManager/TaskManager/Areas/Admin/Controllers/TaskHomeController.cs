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
    public class TaskHomeController : Controller
    {
        // GET: Admin/TaskHome
        public ActionResult Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Index");
            //}

            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int year = DateTime.Now.Year;

            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];

            string res = year + "年" + month + "月" + day + "日" + "，" + week;

            ViewBag.daytime = res;

            return View();
        }

        public ActionResult UpdatePwd()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Login/Index");
            //}
            return View();
        }

        public void UpPwd(string oldpwd, string newpwd1, string newpwd2)
        {
            string newId = Session["UserId"].ToString()  ;
            int id= Convert.ToInt32(newId);
            DALT_Base_Admin dal = new DALT_Base_Admin();
            T_Base_Admin admin = dal.GetModel(id);

            string a = MD5Class.UserMd5(oldpwd);
            string b = MD5Class.UserMd5(newpwd1);

            if (a != admin.PassWord)
            {
                string tmp = "{\"statusCode\":\"300\",\"message\":\"旧密码错误\",\"navTabId\":\"UserList\",\"rel\":\"UserList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                Response.Write(tmp);
            }
            else
            {
                if (newpwd1 != newpwd2)
                {
                    string tmp = "{\"statusCode\":\"300\",\"message\":\"新密码不一致\",\"navTabId\":\"UserList\",\"rel\":\"UserList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                    Response.Write(tmp);
                }
                else
                {
                    admin.PassWord = b;
                    bool res = dal.Update(admin);

                    if (res)
                    {
                        string tmp = "{\"statusCode\":\"200\",\"message\":\"修改成功\",\"navTabId\":\"UserList\",\"rel\":\"UserList\",\"callbackType\":\"closeCurrent\",\"forwardUrl\":\"\"}";
                        Response.Write(tmp);
                    }
                    else
                    {
                        string tmp = "{\"statusCode\":\"300\",\"message\":\"修改失败,请重试\",\"navTabId\":\"UserList\",\"rel\":\"UserList\",\"callbackType\":\"\",\"forwardUrl\":\"\"}";
                        Response.Write(tmp);
                    }
                }
            }

        }
    }
}