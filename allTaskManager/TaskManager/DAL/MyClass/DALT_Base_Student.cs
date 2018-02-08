using Maticsoft.DBUtility;
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
    public partial class DALT_Base_Student
    {
        public List<T_Base_Student> GetAllStudents(int classid)
        {
            string where = "ClassId = " + classid;

            DataSet ds = GetList(where);

            List<T_Base_Student> lst = new List<T_Base_Student>();

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
            cm.CommandText = "select count(1) from t_base_student where " + where;

            int record = (int)cm.ExecuteScalar();

            return record;
        }

        public List<T_Base_Student> GetStudentList(int pageSize, int pageIndex, string where)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.Connection = co;
            cm.CommandText = "select top " + pageSize + " * from V_Class_Student where " + where + " and id not in(select top " + (pageIndex - 1) * pageSize + " id from V_Class_Student where " + where + ")";


            SqlDataReader dr = cm.ExecuteReader();
            List<T_Base_Student> lst = new List<T_Base_Student>();
            while (dr.Read())
            {
                #region 模式转换
                T_Base_Student student = new T_Base_Student();
                T_Base_Class cla = new T_Base_Class();
                student.Id = Convert.ToInt32(dr["Id"]);
                student.StuId = Convert.ToString(dr["StuId"]);
                student.Name = Convert.ToString(dr["Name"]);
                student.Sex = Convert.ToInt32(dr["Sex"]);
                student.PassWord = Convert.ToString(dr["PassWord"]);
                student.Phone = Convert.ToString(dr["Phone"]);
                student.IsBGB = Convert.ToInt32(dr["IsBGB"]);
                student.IsKDB = Convert.ToInt32(dr["IsKDB"]);
                student.ClassId = Convert.ToInt32(dr["ClassId"]);
                cla.Name = Convert.ToString(dr["className"]);
                student.Class = cla;
                #endregion

                lst.Add(student);
            }

            co.Close();
            dr.Close();

            return lst;
        }

        public T_Base_Student GetStudent(int Id)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from V_Class_Student where Id=@Id";
            cm.Parameters.AddWithValue("@Id", Id);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            T_Base_Student student = null;
            T_Base_Class cla = null;
            while (dr.Read())
            {
                #region 模式转换
                student = new T_Base_Student();
                cla = new T_Base_Class();
                cla.Name = Convert.ToString(dr["className"]);
                student.Id = Convert.ToInt32(dr["Id"]);
                student.StuId = Convert.ToString(dr["StuId"]);
                student.Name = Convert.ToString(dr["Name"]);
                student.Sex = Convert.ToInt32(dr["Sex"]);
                student.PassWord = Convert.ToString(dr["PassWord"]);
                student.Phone = Convert.ToString(dr["Phone"]);
                student.IsBGB = Convert.ToInt32(dr["IsBGB"]);
                student.IsKDB = Convert.ToInt32(dr["IsKDB"]);
                student.ClassId = Convert.ToInt32(dr["ClassId"]);
                student.Class = cla;
                #endregion

            }

            dr.Close();
            co.Close();

            return student;
        }

        public T_Base_Student GetStu(String StuId)
        {
            SqlConnection co = new SqlConnection();
            co.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["dataConnection"];
            co.Open();

            SqlCommand cm = new SqlCommand();
            cm.CommandText = "select * from T_Base_Student where StuId=@StuId";
            cm.Parameters.AddWithValue("@StuId", StuId);
            cm.Connection = co;

            SqlDataReader dr = cm.ExecuteReader();
            T_Base_Student student = null;
            while (dr.Read())
            {
                #region 模式转换
                student = new T_Base_Student();
                student.Id = Convert.ToInt32(dr["Id"]);
                student.StuId = Convert.ToString(dr["StuId"]);
                student.Name = Convert.ToString(dr["Name"]);
                student.Sex = Convert.ToInt32(dr["Sex"]);
                student.PassWord = Convert.ToString(dr["PassWord"]);
                student.Phone = Convert.ToString(dr["Phone"]);
                student.IsBGB = Convert.ToInt32(dr["IsBGB"]);
                student.IsKDB = Convert.ToInt32(dr["IsKDB"]);
                student.ClassId = Convert.ToInt32(dr["ClassId"]);
                #endregion

            }

            dr.Close();
            co.Close();

            return student;
        }

        public DataSet GetListByPageByView(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.Id desc");
            }
            strSql.Append(")AS Row, T.*  from V_Class_Student T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        public List<T_Base_Student> GetStuListByView(string where, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPageByView(where, "id", startIndex, endIndex);

            List<T_Base_Student> lst = new List<T_Base_Student>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //lst.Add(DataRowToModel(dr));

                T_Base_Student student = new T_Base_Student();
                student = GetModel((int)dr["Id"]);

                T_Base_Class cla = new T_Base_Class();

                cla.Name = Convert.ToString(dr["className"]);




                student.Class = cla;

                lst.Add(student);

            }

            return lst;
        }

        public DataSet GetListByView(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,className,Name,ClassId");
            strSql.Append(" FROM V_Class_Student ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
    }
}
