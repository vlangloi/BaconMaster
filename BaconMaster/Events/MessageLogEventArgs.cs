using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaconMaster.Events
{
	
	public class MessageLogEventArgs : EventArgs
	{
		public enum LevelType
		{
			DEBUG,
			INFO,
			WARN,
			ERROR
		}

		public MessageLogEventArgs(string msg, LevelType level)
		{
			_msg = msg;
			_level = level;
            _timeStamp = DateTime.Now;
		}

		private string _msg;
		public string Msg
		{
			get { return _msg; }
		}

		private LevelType _level;
		public LevelType Level
		{
			get { return _level; }
		}

        private DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
        }
	}
}
