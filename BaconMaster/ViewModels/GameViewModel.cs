using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace BaconMaster.ViewModels
{
	public class GameViewModel : PropertyChangedBase
	{
		public GameViewModel(GameManager model)
		{
			_model = model;

			_model.RoundChangedEvent += HandleRoundChanged;
			_model.ActivePlayerChangedEvent += HandleActivePlayerChanged;
		}

		private GameManager _model;

		public int CurrentRound
		{
			get { return _model.CurrentRound; }
			//set { _currentRound = value; NotifyOfPropertyChange(() => CurrentRound); }
		}

		public string CurrentActivePlayer
		{
			get { return _model.PlayerManager.GetActivePlayer().Name; }
			//set { _currentRound = value; NotifyOfPropertyChange(() => CurrentActivePlayer); }
		}
		#region Buttons

		public void RollAll()
		{
			_model.RollAll();
		}
		public void LockDiceWithRule()
		{
			_model.LockDiceWithRule();
		}

		public void Reroll()
		{
			_model.Reroll();
		}

		public void ApplyDiceResult()
		{
			_model.ApplyDiceResult();
		}

		public void NextPlayerTurn()
		{
			_model.NextPlayerTurn();
		}

		public void ExecutePlayerTurn()
		{
			_model.ExecutePlayerTurn();
		}
		public void ExecuteFullRound()
		{
			_model.ExecuteFullRound();
		}

		public void PlayFullGame()
		{
			// if already running
			// Partir dans une tache(thread) pour ne pas bloquer le visuel
			var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
			{
				_model.ExecutePlayFullGame();
			});
		}

		#endregion

		#region Handles

		protected void HandleRoundChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => CurrentRound);
		}

		protected void HandleActivePlayerChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => CurrentActivePlayer);
		}
		#endregion
	}
}
