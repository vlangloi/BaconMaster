using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaconMaster.Events;
using BaconMaster.Models;


namespace BaconMaster
{
	public class GameManager : IMessageLog
	{
		public GameManager()
		{
			BaconPointToWin = Properties.Settings.Default.BaconPointToWin;
			int nbPlayer = Properties.Settings.Default.NumberPlayer;
			int initialHitPoint = Properties.Settings.Default.InitialHitPoint;
			int initialBaconPoint = Properties.Settings.Default.InitialBaconPoint;
			PlayerManager = new PlayerManager(nbPlayer, initialHitPoint, initialBaconPoint);
			DiceManager = new DiceManager(12);

			// S'abonner quand on a fait le tour de tous les joueurs.
			PlayerManager.ActivePlayerChangedEvent += HandleActivePlayerChanged;
			PlayerManager.EveryonePlayedEvent += HandleEveryonePlayed;
		}

		private PlayerManager _playerManager;
		public PlayerManager PlayerManager
		{
			get { return _playerManager; }
			protected set { _playerManager = value; }
		}

		private DiceManager _diceManager;
		public DiceManager DiceManager
		{
			get { return _diceManager; }
			protected set { _diceManager = value; }
		}
		private int _currentRound = 1;
		public int CurrentRound
		{
			get { return _currentRound; }
			set 
			{
				if (value.Equals(_currentRound)) return;
				_currentRound = value; 
				OnRoundChange(); 
				OnMessage(String.Format("***Round change to {0}***", _currentRound));
			}
		}

		private int _baconPointToWin = 25;
		public int BaconPointToWin
		{
			get { return _baconPointToWin; }
			set { _baconPointToWin = value; }
		}

		private bool _isGameDone = false;
		public bool IsGameDone
		{
			get { return _isGameDone; }
			set { _isGameDone = value; }
		}

		#region Public Methods
		public void RollAll()
		{
			//OnMessage("Executing RollAll()", MessageLogEventArgs.LevelType.INFO);
			DiceManager.RollAll();
			OnMessage(String.Format("{0} roll all => {1}", _playerManager.GetActivePlayer().Name, DiceManager.ToString()));
			//OnMessage("Roll All " + DiceManager.ToString());
		}

		public void LockDiceWithRule()
		{
			LockDicesRule();
		}

		public void Reroll()
		{			
			DiceManager.RollUnlocked();
			OnMessage(String.Format("{0} reroll => {1}", _playerManager.GetActivePlayer().Name, DiceManager.ToString()));
			//OnMessage("Reroll " + DiceManager.ToString());
		}

		public void ApplyDiceResult()
		{
			OnMessage("Executing ApplyDiceResult()");
			// 1 - Bacon +1
			// 2 - HP -1
			// 3 - HP +1 
			// 4 - Bacon +3, HP-2
			// 5 - Bacon -1 to highest BP. Si equals HP, sinon random
			// 6 - HP -1 to highest BP
			int hitPointChange = 0;
			int baconPointChange = 0;
			
			int highestPlayerHitPointChange = 0;
			int highestPlayerBaconPointChange = 0;
			foreach (Dice dice in _diceManager.DiceArray)
			{
				if (dice.Result == 1)
				{
					baconPointChange++;
				}
				if (dice.Result == 2)
				{
					hitPointChange--;
				}
				if (dice.Result == 3)
				{
					hitPointChange++;
				}
				if (dice.Result == 4)
				{
					baconPointChange += 3;
					hitPointChange -= 2;
				}
				if (dice.Result == 5)
				{
					highestPlayerBaconPointChange--;
				}
				if (dice.Result == 6)
				{
					highestPlayerHitPointChange--;
				}
			}
			_playerManager.GetActivePlayer().HitPoint += hitPointChange;
			_playerManager.GetActivePlayer().BaconPoint += baconPointChange;

			OnMessage(String.Format("{0} => {1} HitPoint", _playerManager.GetActivePlayer().Name, hitPointChange));
			OnMessage(String.Format("{0} => {1} BaconPoint", _playerManager.GetActivePlayer().Name, baconPointChange));

			string highestPlayerName = "TODO Highest Player";
			OnMessage(String.Format("{0} => {1} HitPoint", highestPlayerName, highestPlayerHitPointChange));
			OnMessage(String.Format("{0} => {1} BaconPoint", highestPlayerName, highestPlayerBaconPointChange));
		}

		public void NextPlayerTurn()
		{
			OnMessage("Executing NextPlayerTurn()");
			// Changer currentPlayer
			if (!_playerManager.ActivateNextPlayer())
			{
				CheckEndGame();
			} 		
		}

		public void ExecutePlayerTurn()
		{
			if (_playerManager.NumberPlayerAlive() > 1)
			{
				RollAll();
				LockDiceWithRule();
				Reroll();
				ApplyDiceResult();
				NextPlayerTurn();
				OnMessage("--------------------");
			}
			else
			{
				CheckEndGame();
			}
		}

		public void ExecuteFullRound()
		{
			int currentRoundTemp = _currentRound;
			while (currentRoundTemp == CurrentRound && _playerManager.NumberPlayerAlive() > 1)
			{
				ExecutePlayerTurn();
			}			
		}

		public void ExecutePlayFullGame()
		{
			//TODO maxIteration
			int maxIteration = 100;
			int counter = 0;
			while (!IsGameDone)
			{
				ExecuteFullRound();
				if (counter > maxIteration)
				{
					OnMessage("Too much iteration PlayFullGame.");
					break;
				}
				counter++;
			}		
		}

		public void NewGame()
		{
			IsGameDone = false;
			// Reset Player
			int initialHitPoint = Properties.Settings.Default.InitialHitPoint;
			int initialBaconPoint = Properties.Settings.Default.InitialBaconPoint;
			foreach (Player player in _playerManager.AllPlayerList())
			{
				player.HitPoint = initialHitPoint;
				player.BaconPoint = initialBaconPoint;
			}

			// Change Active Player
			_playerManager.ResetToFirstActivePlayer();

			// Reset Round
			CurrentRound = 1;
		}

		#endregion

		#region Private Methods		

		private void LockDicesRule()
		{
			int[] counter = new int[6] { 0,0,0,0,0,0 };
			foreach (Dice item in DiceManager.DiceArray)
			{
				if (item.Result == 1)
				{
					counter[0]++;
				}
				else if (item.Result == 2)
				{
					counter[1]++;
				}
				else if (item.Result == 3)
				{
					counter[2]++;
				}
				else if (item.Result == 4)
				{
					counter[3]++;
				}
				else if (item.Result == 5)
				{
					counter[4]++;
				}
				else if (item.Result == 6)
				{
					counter[5]++;
				}
				else
				{
					OnMessage("LockDicesRule()=> Error", MessageLogEventArgs.LevelType.ERROR);
				}
			}
			foreach (Dice dice in DiceManager.DiceArray)
			{
				int diceResult = dice.Result - 1;
				if (counter[diceResult] > 1)
				{
					dice.IsLocked = false;
				}
				else
				{
					dice.IsLocked = true;
					OnMessage("Keeping Dice Result value = " + dice.Result);
				}
			}
		}

		private void CheckEndGame()
		{
			if (_playerManager.NumberPlayerAlive() <= 1)
			{
				OnMessage("*****Game Ended*****");
				// Check Winner
				if (_playerManager.NumberPlayerAlive() == 0)
				{
					OnMessage("NO WINNER :(");
					IsGameDone = true;
				}
				else
				{
					// Get Highest HP player
					OnMessage("WINNER => " + _playerManager.GetActivePlayer().Name);	//TODO Devrait être le plus de HP
					IsGameDone = true;
				}
			}
			else
			{
				// Bacon point
				// Get HighestAliveBaconPointPlayer
				foreach (Player player in _playerManager.AlivePlayerList())
				{
					if (player.BaconPoint >= BaconPointToWin)
					{
						OnMessage("WINNER => " + player.Name);
						IsGameDone = true;
					}
				}
			}			
		}

		#endregion

		#region Events

		public event EventHandler RoundChangedEvent;
		protected void OnRoundChange()
		{
			EventHandler handler = RoundChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		public event EventHandler ActivePlayerChangedEvent;
		protected void OnActivePlayerChange()
		{
			EventHandler handler = ActivePlayerChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		public event EventHandler<MessageLogEventArgs> MessageLogEvent;
		protected void OnMessage(string msg, MessageLogEventArgs.LevelType level = MessageLogEventArgs.LevelType.INFO)
		{
			EventHandler<MessageLogEventArgs> handler = MessageLogEvent;
			if (handler != null)
			{
				handler(this, new MessageLogEventArgs(msg, level));
			}
		}
		#endregion

		#region Handles

		protected void HandleActivePlayerChanged(object sender, EventArgs e)
		{
			OnMessage(String.Format("*New Active Player = {0}", _playerManager.GetActivePlayer().Name));
			// Renvoyer l'event
			OnActivePlayerChange();
		}

		protected void HandleEveryonePlayed(object sender, EventArgs e)
		{
			CurrentRound++;
			CheckEndGame();
		}

		#endregion
	}
}
