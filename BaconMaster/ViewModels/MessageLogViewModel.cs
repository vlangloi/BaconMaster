using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BaconMaster.Events;

namespace BaconMaster.ViewModels
{
	public class MessageLogViewModel : PropertyChangedBase, IDisposable
	{
		#region Nested Class
		public class InfoMessage
		{
			public InfoMessage(string msg, bool isError)
			{
				Msg = msg;
				IsError = isError;
			}
			private string _msg;
			public string Msg
			{
				get { return _msg; }
				private set { _msg = value; }
			}
			private bool _isError;
			public bool IsError
			{
				get { return _isError; }
				private set { _isError = value; }
			}
		}
		#endregion

		public MessageLogViewModel()
			: this(500)
		{
			
		}

		public MessageLogViewModel(int maxDisplayMessage)
		{
			_messageConsumerTask = Task.Factory.StartNew(() => AnalyzerMsgConsumerThread(), TaskCreationOptions.LongRunning);
			_maxCountList = maxDisplayMessage;
		}

		public void Dispose()
		{
			_messageLogs.CompleteAdding();	//Débloquer le thread.

			try
			{
				_messageConsumerTask.Wait(2000);			
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten();	// The Flatten method is used to extract the inner exceptions from any nested AggregateException instances and re-throw a single AggregateException that directly contains all the inner unhandled exceptions. Flattening the exception makes it more convenient for client code to handle.
			}
		}

		private BindableCollection<InfoMessage> _infoMessages = new BindableCollection<InfoMessage>();
		public BindableCollection<InfoMessage> InfoMessages
		{
			get { return _infoMessages; }
			set
			{
				_infoMessages = value;
				NotifyOfPropertyChange(() => InfoMessages);
			}
		}


		private readonly int _maxCountList = 500;

		private void AddInfoMessage(string msg, bool isError)
		{
			string msgWithDate = DateTime.Now + " - " + msg;
			InfoMessage infoMsg = new InfoMessage(msgWithDate, isError);
			InfoMessages.Add(infoMsg);

			if (InfoMessages.Count >= _maxCountList)
			{
				InfoMessages.RemoveAt(0);
			}
		}

		#region Messages Consumer

		private BlockingCollection<MessageLogEventArgs> _messageLogs = new BlockingCollection<MessageLogEventArgs>();
		private Task _messageConsumerTask;

		// Producer
		public void AddMessage(MessageLogEventArgs msg)
		{
			if (!_messageLogs.IsCompleted)
			{
				_messageLogs.Add(msg);
			}
		}

		private void AnalyzerMsgConsumerThread()
		{
			// GetConsumingEnumerable() will block until you there's more to process or 
			// you tell the collection you're done adding.  You can have 
			// multiple threads doing the adding and the consuming, as well.
			foreach (MessageLogEventArgs item in _messageLogs.GetConsumingEnumerable())
			{
				if (item.Level == MessageLogEventArgs.LevelType.ERROR)
				{
					AddInfoMessage(item.Msg, true);				
				}
				else
				{
					AddInfoMessage(item.Msg, false);
				}				
			}
		}
		#endregion
	}
}
