using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;
using System.Data.SqlClient;

namespace TaskManager.DAL
{
    public partial class DALT_ClassTask_Stu
    {
        public bool ExistsStu(int stuId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from T_ClassTask_Stu");
            strSql.Append(" where StuId=@stuId");
            SqlParameter[] parameters = {
                    new SqlParameter("@stuId", SqlDbType.Int,4)
            };
            parameters[0].Value = stuId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        public bool DeleteWhere(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_ClassTask_Stu ");
            strSql.Append(" where " + where);
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetPerm(string where)
        {
            DALT_ClassTask_Stu dal = new DALT_ClassTask_Stu();
            DataSet ds = dal.GetList(where);

            List<T_ClassTask_Stu> lst = new List<T_ClassTask_Stu>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(DataRowToModel(dr));
            }

            if (lst.Count() == 0)
                return false;
            else
                return true;
        }
    }
}
