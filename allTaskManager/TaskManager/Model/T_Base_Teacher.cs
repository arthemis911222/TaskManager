using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Base_Teacher:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Base_Teacher
    {
        public T_Base_Teacher()
        { }
        #region Model
        private int _id;
        private string _teaid;
        private string _name;
        private int _sex = 0;
        private string _password;
        private string _phone;
        private int _isbzr = 0;
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
        public string TeaId
        {
            set { _teaid = value; }
            get { return _teaid; }
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
        public int IsBZR
        {
            set { _isbzr = value; }
            get { return _isbzr; }
        }
        #endregion Model

    }
}

