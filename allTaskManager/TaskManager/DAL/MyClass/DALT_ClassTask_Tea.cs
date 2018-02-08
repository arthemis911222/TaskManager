using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.DAL
{
    public partial class DALT_ClassTask_Tea
    {
        public bool DeleteWhere(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_ClassTask_Tea ");
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
            DALT_ClassTask_Tea dal = new DALT_ClassTask_Tea();
            DataSet ds = dal.GetList(where);

            List<T_ClassTask_Tea> lst = new List<T_ClassTask_Tea>();

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
