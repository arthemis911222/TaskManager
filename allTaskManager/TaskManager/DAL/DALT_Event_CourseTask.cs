using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace TaskManager.DAL
{
    /// <summary>
    /// 数据访问类:T_Event_CourseTask
    /// </summary>
    public partial class DALT_Event_CourseTask
    {
        public DALT_Event_CourseTask()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from T_Event_CourseTask");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(TaskManager.Model.T_Event_CourseTask model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_Event_CourseTask(");
            strSql.Append("StartTime,EndTime,Type,Description,StartWeek,WeekType,WeekNum,IsAlert,State,CourseId,WPeople)");
            strSql.Append(" values (");
            strSql.Append("@StartTime,@EndTime,@Type,@Description,@StartWeek,@WeekType,@WeekNum,@IsAlert,@State,@CourseId,@WPeople)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@StartTime", SqlDbType.NVarChar,50),
                    new SqlParameter("@EndTime", SqlDbType.NVarChar,50),
                    new SqlParameter("@Type", SqlDbType.Int,4),
                    new SqlParameter("@Description", SqlDbType.Text),
                    new SqlParameter("@StartWeek", SqlDbType.DateTime),
                    new SqlParameter("@WeekType", SqlDbType.Int,4),
                    new SqlParameter("@WeekNum", SqlDbType.Int,4),
                    new SqlParameter("@IsAlert", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@CourseId", SqlDbType.Int,4),
                    new SqlParameter("@WPeople", SqlDbType.NVarChar,20)};
            parameters[0].Value = model.StartTime;
            parameters[1].Value = model.EndTime;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.StartWeek;
            parameters[5].Value = model.WeekType;
            parameters[6].Value = model.WeekNum;
            parameters[7].Value = model.IsAlert;
            parameters[8].Value = model.State;
            parameters[9].Value = model.CourseId;
            parameters[10].Value = model.WPeople;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(TaskManager.Model.T_Event_CourseTask model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update T_Event_CourseTask set ");
            strSql.Append("StartTime=@StartTime,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("Type=@Type,");
            strSql.Append("Description=@Description,");
            strSql.Append("StartWeek=@StartWeek,");
            strSql.Append("WeekType=@WeekType,");
            strSql.Append("WeekNum=@WeekNum,");
            strSql.Append("IsAlert=@IsAlert,");
            strSql.Append("State=@State,");
            strSql.Append("CourseId=@CourseId,");
            strSql.Append("WPeople=@WPeople");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@StartTime", SqlDbType.NVarChar,50),
                    new SqlParameter("@EndTime", SqlDbType.NVarChar,50),
                    new SqlParameter("@Type", SqlDbType.Int,4),
                    new SqlParameter("@Description", SqlDbType.Text),
                    new SqlParameter("@StartWeek", SqlDbType.DateTime),
                    new SqlParameter("@WeekType", SqlDbType.Int,4),
                    new SqlParameter("@WeekNum", SqlDbType.Int,4),
                    new SqlParameter("@IsAlert", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@CourseId", SqlDbType.Int,4),
                    new SqlParameter("@WPeople", SqlDbType.NVarChar,20),
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = model.StartTime;
            parameters[1].Value = model.EndTime;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.StartWeek;
            parameters[5].Value = model.WeekType;
            parameters[6].Value = model.WeekNum;
            parameters[7].Value = model.IsAlert;
            parameters[8].Value = model.State;
            parameters[9].Value = model.CourseId;
            parameters[10].Value = model.WPeople;
            parameters[11].Value = model.Id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_Event_CourseTask ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_Event_CourseTask ");
            strSql.Append(" where Id in (" + Idlist + ")  ");
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


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TaskManager.Model.T_Event_CourseTask GetModel(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Id,StartTime,EndTime,Type,Description,StartWeek,WeekType,WeekNum,IsAlert,State,CourseId,WPeople from T_Event_CourseTask ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;

            TaskManager.Model.T_Event_CourseTask model = new TaskManager.Model.T_Event_CourseTask();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TaskManager.Model.T_Event_CourseTask DataRowToModel(DataRow row)
        {
            TaskManager.Model.T_Event_CourseTask model = new TaskManager.Model.T_Event_CourseTask();
            if (row != null)
            {
                if (row["Id"] != null && row["Id"].ToString() != "")
                {
                    model.Id = int.Parse(row["Id"].ToString());
                }
                if (row["StartTime"] != null)
                {
                    model.StartTime = row["StartTime"].ToString();
                }
                if (row["EndTime"] != null)
                {
                    model.EndTime = row["EndTime"].ToString();
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["StartWeek"] != null && row["StartWeek"].ToString() != "")
                {
                    model.StartWeek = DateTime.Parse(row["StartWeek"].ToString());
                }
                if (row["WeekType"] != null && row["WeekType"].ToString() != "")
                {
                    model.WeekType = int.Parse(row["WeekType"].ToString());
                }
                if (row["WeekNum"] != null && row["WeekNum"].ToString() != "")
                {
                    model.WeekNum = int.Parse(row["WeekNum"].ToString());
                }
                if (row["IsAlert"] != null && row["IsAlert"].ToString() != "")
                {
                    model.IsAlert = int.Parse(row["IsAlert"].ToString());
                }
                if (row["State"] != null && row["State"].ToString() != "")
                {
                    model.State = int.Parse(row["State"].ToString());
                }
                if (row["CourseId"] != null && row["CourseId"].ToString() != "")
                {
                    model.CourseId = int.Parse(row["CourseId"].ToString());
                }
                if (row["WPeople"] != null)
                {
                    model.WPeople = row["WPeople"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,StartTime,EndTime,Type,Description,StartWeek,WeekType,WeekNum,IsAlert,State,CourseId,WPeople ");
            strSql.Append(" FROM T_Event_CourseTask ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" Id,StartTime,EndTime,Type,Description,StartWeek,WeekType,WeekNum,IsAlert,State,CourseId,WPeople ");
            strSql.Append(" FROM T_Event_CourseTask ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM T_Event_CourseTask ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
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
            strSql.Append(")AS Row, T.*  from T_Event_CourseTask T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "T_Event_CourseTask";
			parameters[1].Value = "Id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

