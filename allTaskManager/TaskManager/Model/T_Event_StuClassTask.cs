using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Event_StuClassTask:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Event_StuClassTask
    {
        public T_Event_StuClassTask()
        { }
        #region Model
        private int _id;
        private int _classtaskid;
        private int _stuid;
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
        public int ClassTaskId
        {
            set { _classtaskid = value; }
            get { return _classtaskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StuId
        {
            set { _stuid = value; }
            get { return _stuid; }
        }
        #endregion Model

    }
}

