using System;
namespace TaskManager.Model
{
    /// <summary>
    /// T_Event_MyTask:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_Event_MyTask
    {
        public T_Event_MyTask()
        { }
        #region Model
        private int _id;
        private string _name;
        private DateTime? _starttime;
        private DateTime? _endtime;
        private int _type = 1;
        private string _description;
        private int _isalert = 0;
        private int? _alerttime;
        private int _state = 0;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
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
        public int? AlertTime
        {
            set { _alerttime = value; }
            get { return _alerttime; }
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
        public int StuId
        {
            set { _stuid = value; }
            get { return _stuid; }
        }
        #endregion Model

    }
}

