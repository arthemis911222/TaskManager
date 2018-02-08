using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Event_CourseTask:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Event_CourseTask
    {
        public T_Event_CourseTask()
        { }
        #region Model
        private int _id;
        private string _starttime;
        private string _endtime;
        private int _type = 1;
        private string _description;
        private DateTime? _startweek;
        private int _weektype = 0;
        private int? _weeknum;
        private int _isalert = 0;
        private int _state = 0;
        private int _courseid;
        private string _wpeople;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 开始时间（时分）
        /// </summary>
        public string StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 结束时间（时分）
        /// </summary>
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
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
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 开始日期（年月日）
        /// </summary>
        public DateTime? StartWeek
        {
            set { _startweek = value; }
            get { return _startweek; }
        }
        /// <summary>
        /// 单双周
        /// </summary>
        public int WeekType
        {
            set { _weektype = value; }
            get { return _weektype; }
        }
        /// <summary>
        /// 持续周数
        /// </summary>
        public int? WeekNum
        {
            set { _weeknum = value; }
            get { return _weeknum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsAlert
        {
            set { _isalert = value; }
            get { return _isalert; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CourseId
        {
            set { _courseid = value; }
            get { return _courseid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WPeople
        {
            set { _wpeople = value; }
            get { return _wpeople; }
        }
        #endregion Model

    }
}

