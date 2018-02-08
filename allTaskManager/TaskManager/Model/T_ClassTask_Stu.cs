using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_ClassTask_Stu:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_ClassTask_Stu
    {
        public T_ClassTask_Stu()
        { }
        #region Model
        private int _id;
        private int _classid;
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
        public int ClassId
        {
            set { _classid = value; }
            get { return _classid; }
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

