using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DAL
{
    public partial class DALT_Base_Admin
    {
        public T_Base_Admin GetAdmin(String LoginName)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from T_Base_Admin where LoginName=@LoginName";
            cm.Parameters.AddWithValue("@LoginName", LoginName);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            T_Base_Admin admin = null;
            while (dr.Read())
            {
                #region 模式转换
                admin = new T_Base_Admin();
                admin.Id = Convert.ToInt32(dr["Id"]);
                admin.LoginName = Convert.ToString(dr["LoginName"]);
                admin.PassWord = Convert.ToString(dr["PassWord"]);                
                #endregion

            }

            dr.Close();
            co.Close();

            return admin;
        }
    }
}
