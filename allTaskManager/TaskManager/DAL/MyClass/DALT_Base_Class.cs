using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DAL
{
    public partial class DALT_Base_Class
    {
        public Model.T_Base_Class FindCla(int TeaId)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from T_Base_class where TeaId=@TeaId";
            cm.Parameters.AddWithValue("@TeaId", TeaId);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            Model.T_Base_Class cla = null;
            while (dr.Read())
            {
                #region 模式转换
                cla = new Model.T_Base_Class();
                cla.Id = Convert.ToInt32(dr["Id"]);
                cla.Name = Convert.ToString(dr["Name"]);
                #endregion

            }

            dr.Close();
            co.Close();
            return cla;
        }

        public List<Model.T_Base_Class> GetAllList(string where)
        {
            DataSet ds = GetList(where);

            List<Model.T_Base_Class> lst = new List<Model.T_Base_Class>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        public int GetRecord(string where)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.Connection = co;
            cm.CommandText = "select count(1) from t_base_class where " + where;

            int record = (int)cm.ExecuteScalar();

            return record;
        }

        public List<T_Base_Class> GetClassList(int pageSize, int pageIndex, string where)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.Connection = co;
            cm.CommandText = "select top " + pageSize + " * from V_Class_Teacher where " + where + " and id not in(select top " + (pageIndex - 1) * pageSize + " id from V_Class_Teacher where " + where + ")";


            SqlDataReader dr = cm.ExecuteReader();
            List<T_Base_Class> lst = new List<T_Base_Class>();
            while (dr.Read())
            {
                #region 模式转换
                T_Base_Class cla = new T_Base_Class();
                T_Base_Teacher teacher = new T_Base_Teacher();               
                cla.Id = Convert.ToInt32(dr["classId"]);
                cla.Name = Convert.ToString(dr["className"]);                
                cla.TeaId = Convert.ToInt32(dr["Id"]);

                teacher.Name = Convert.ToString(dr["Name"]);

                cla.Teacher = teacher;
                #endregion

                lst.Add(cla);
            }

            co.Close();
            dr.Close();

            return lst;
        }

        public bool Add(T_Base_Class cla)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "insert into T_Base_Class(TeaId,Name) Values(@TeaId,@Name)";
            cm.Connection = co;

            #region 变量赋值
            cm.Parameters.AddWithValue("@TeaId", cla.TeaId);
            cm.Parameters.AddWithValue("@Name", cla.Name);           
            #endregion

            int res = cm.ExecuteNonQuery();

            co.Close();

            if (res > 0)
                return true;
            else
                return false;

        }

        public T_Base_Class GetClass(int Id)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from V_Class_Teacher where classId=@Id";
            cm.Parameters.AddWithValue("@Id", Id);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            T_Base_Teacher teacher = null;
            T_Base_Class cla = null;
            while (dr.Read())
            {
                #region 模式转换
                cla = new T_Base_Class();
                teacher = new T_Base_Teacher();
                cla.Id = Convert.ToInt32(dr["classId"]);
                cla.Name = Convert.ToString(dr["className"]);
                cla.TeaId = Convert.ToInt32(dr["Id"]);

                teacher.Name = Convert.ToString(dr["Name"]);

                cla.Teacher = teacher;
                #endregion

            }

            dr.Close();
            co.Close();

            return cla;
        }
    }
}
