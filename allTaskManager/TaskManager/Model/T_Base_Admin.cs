using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Base_Admin:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Base_Admin
    {
        public T_Base_Admin()
        { }
        #region Model
        private int _id;
        private string _loginname;
        private string _password;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName
        {
            set { _loginname = value; }
            get { return _loginname; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord
        {
            set { _password = value; }
            get { return _password; }
        }
        #endregion Model

    }
}

