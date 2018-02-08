using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Base_Student:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Base_Student
    {
        public T_Base_Student()
        { }
        #region Model
        private int _id;
        private string _stuid;
        private string _name;
        private int _sex = 0;
        private string _password;
        private string _phone;
        private int _isbgb = 0;
        private int _iskdb = 0;
        private int _classid;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StuId
        {
            set { _stuid = value; }
            get { return _stuid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PassWord
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsBGB
        {
            set { _isbgb = value; }
            get { return _isbgb; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsKDB
        {
            set { _iskdb = value; }
            get { return _iskdb; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ClassId
        {
            set { _classid = value; }
            get { return _classid; }
        }
        #endregion Model

    }
}

