using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaconMaster.Events
{
	public interface IMessageLog
	{
		event EventHandler<MessageLogEventArgs> MessageLogEvent;
	}
}
