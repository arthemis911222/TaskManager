using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_ClassTask_Tea:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_ClassTask_Tea
    {
        public T_ClassTask_Tea()
        { }
        #region Model
        private int _id;
        private int _classid;
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
        public int ClassId
        {
            set { _classid = value; }
            get { return _classid; }
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

