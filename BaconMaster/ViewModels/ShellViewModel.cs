using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using BaconMaster.Models;
using BaconMaster.Events;

namespace BaconMaster.ViewModels
{
	[Export(typeof(ShellViewModel))]
	public class ShellViewModel : Screen
	{
		private readonly IWindowManager _windowManager;
		private readonly IEventAggregator _events;
		//private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		[ImportingConstructor]
		public ShellViewModel(IWindowManager windowManager,
			IEventAggregator events)
		{
			this._events = events;
			_events.Subscribe(this);
			this._windowManager = windowManager;

			DisplayName = "BaconMaster Tester";

			_currentGame = new GameManager();

			GamePanel = new GameViewModel(_currentGame);
			AllPlayerPanel = new AllPlayerViewModel(_currentGame.PlayerManager.AllPlayerList());
			MessageLogPanel = new MessageLogViewModel();

			_currentGame.MessageLogEvent += HandleMessageLogEvent;
		}

		private GameManager _currentGame;

		public AllPlayerViewModel AllPlayerPanel { get; protected set; }
		public GameViewModel GamePanel { get; protected set; }
		public MessageLogViewModel MessageLogPanel { get; private set; }

		#region Menus
		
		public void NewGame()
		{
			_currentGame.NewGame();
			// TODOLIST
			// Winning condition
			//play until winner thread button
			//checkbox dice
			//apply resul
			//log
			//icon
			//tester nbplayer scrollview

		}

		#endregion

		#region Handles
		public void HandleMessageLogEvent(object sender, MessageLogEventArgs e)
		{
			MessageLogPanel.AddMessage(e);
		}
		#endregion

	}
}
