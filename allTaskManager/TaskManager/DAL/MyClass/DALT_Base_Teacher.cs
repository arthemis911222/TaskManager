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
    public partial class DALT_Base_Teacher
    {
        public T_Base_Teacher GetTea(String TeaId)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from T_Base_Teacher where TeaId=@TeaId";
            cm.Parameters.AddWithValue("@TeaId", TeaId);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            T_Base_Teacher teacher = null;
            while (dr.Read())
            {
                #region 模式转换
                teacher = new T_Base_Teacher();
                teacher.Id = Convert.ToInt32(dr["Id"]);
                teacher.TeaId = Convert.ToString(dr["TeaId"]);
                teacher.Name = Convert.ToString(dr["Name"]);
                teacher.Sex = Convert.ToInt32(dr["Sex"]);
                teacher.PassWord = Convert.ToString(dr["PassWord"]);
                teacher.Phone = Convert.ToString(dr["Phone"]);
                teacher.IsBZR = Convert.ToInt32(dr["IsBZR"]);
                #endregion

            }

            dr.Close();
            co.Close();

            return teacher;
        }

        public int GetRecord(string where)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.Connection = co;
            cm.CommandText = "select count(1) from t_base_teacher where " + where;

            int record = (int)cm.ExecuteScalar();

            return record;
        }

        public List<Model.T_Base_Teacher> GetAllList(string where)
        {
            DataSet ds = GetList(where);

            List<Model.T_Base_Teacher> lst = new List<Model.T_Base_Teacher>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            return lst;
        }

        public List<T_Base_Teacher> GetTeacherList(int pageSize, int pageIndex, string where)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.Connection = co;
            cm.CommandText = "select top " + pageSize + " * from T_Base_Teacher where " + where + " and id not in(select top " + (pageIndex - 1) * pageSize + " id from T_Base_Teacher where " + where + ")";


            SqlDataReader dr = cm.ExecuteReader();
            List<T_Base_Teacher> lst = new List<T_Base_Teacher>();
            while (dr.Read())
            {
                #region 模式转换
                T_Base_Teacher teacher = new T_Base_Teacher();
                teacher.Id = Convert.ToInt32(dr["Id"]);
                teacher.TeaId = Convert.ToString(dr["TeaId"]);
                teacher.Name = Convert.ToString(dr["Name"]);
                teacher.Sex = Convert.ToInt32(dr["Sex"]);
                teacher.PassWord = Convert.ToString(dr["PassWord"]);
                teacher.Phone = Convert.ToString(dr["Phone"]);
                teacher.IsBZR = Convert.ToInt32(dr["IsBZR"]);

                #endregion

                lst.Add(teacher);
            }

            co.Close();
            dr.Close();

            return lst;
        }

        public bool Add(T_Base_Teacher teacher)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "insert into T_Base_Teacher(TeaId,Name,Sex,PassWord,Phone,IsBZR) Values(@TeaId,@Name,@Sex,@PassWord,@Phone,@IsBZR)";
            cm.Connection = co;

            #region 变量赋值
            cm.Parameters.AddWithValue("@TeaId", teacher.TeaId);
            cm.Parameters.AddWithValue("@Name", teacher.Name);
            cm.Parameters.AddWithValue("@Sex", teacher.Sex);
            cm.Parameters.AddWithValue("@PassWord", teacher.PassWord);
            cm.Parameters.AddWithValue("@Phone", teacher.Phone);
            cm.Parameters.AddWithValue("@IsBZR", teacher.IsBZR);

            #endregion

            int res = cm.ExecuteNonQuery();

            co.Close();

            if (res > 0)
                return true;
            else
                return false;

        }

        public T_Base_Teacher GetTeacher(int Id)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from T_Base_Teacher where Id=@Id";
            cm.Parameters.AddWithValue("@Id", Id);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            T_Base_Teacher teacher = null;
            while (dr.Read())
            {
                #region 模式转换
                teacher = new T_Base_Teacher();
                teacher.Id = Convert.ToInt32(dr["Id"]);
                teacher.TeaId = Convert.ToString(dr["TeaId"]);
                teacher.Name = Convert.ToString(dr["Name"]);
                teacher.Sex = Convert.ToInt32(dr["Sex"]);
                teacher.PassWord = Convert.ToString(dr["PassWord"]);
                teacher.Phone = Convert.ToString(dr["Phone"]);
                teacher.IsBZR = Convert.ToInt32(dr["IsBZR"]);

                #endregion

            }

            dr.Close();
            co.Close();

            return teacher;
        }
    }
}
