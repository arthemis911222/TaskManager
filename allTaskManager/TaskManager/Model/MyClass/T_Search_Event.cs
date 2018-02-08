using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model
{
    public class T_Search_Event
    {
        public T_Search_Event()
        { }
        #region Model
        private string _name;
        private DateTime _starttime;
        private DateTime _endtime;
        private int _type = 1;
        private string _description;
        private int _isalert = 0;
        private int _alerttime;
       
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime
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
        public int AlertTime
        {
            set { _alerttime = value; }
            get { return _alerttime; }
        }
        /// <summary>
        /// 
        #endregion Model
    }
}
