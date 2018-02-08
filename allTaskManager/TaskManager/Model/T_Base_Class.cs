using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Base_Class:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Base_Class
    {
        public T_Base_Class()
        { }
        #region Model
        private int _id;
        private string _name;
        private int _teaid;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TeaId
        {
            set { _teaid = value; }
            get { return _teaid; }
        }
        #endregion Model

    }
}

