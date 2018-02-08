using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Task_Course:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Task_Course
    {
        public T_Task_Course()
        { }
        #region Model
        private int _id;
        private string _courseid;
        private string _name;
        private int _type = 0;
        private int? _classid;
        private int? _teaid;
        private int? _stuid;
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
        public string CourseId
        {
            set { _courseid = value; }
            get { return _courseid; }
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
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ClassId
        {
            set { _classid = value; }
            get { return _classid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TeaId
        {
            set { _teaid = value; }
            get { return _teaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? StuId
        {
            set { _stuid = value; }
            get { return _stuid; }
        }
        #endregion Model

    }
}

