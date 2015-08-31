using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaconMaster.Models;

namespace BaconMaster
{
	public class PlayerManager
	{
		public PlayerManager(int nbPlayer, int initialHP, int initialBacon)
		{
			_playerList = new List<Player>();
			_maxPlayer = nbPlayer;

			// TODO Fichier de config des joueurs
			if (nbPlayer > 0)
			{
				for (int i = 0; i < nbPlayer; i++ )
				{
					string playerName = "Player" + (i+1);
					_playerList.Add(new Player(playerName, initialHP, initialBacon));
				}
			}
		}

		private List<Player> _playerList;
		private int _currentPlayerId = 0;
		private int _maxPlayer = 0;

		public bool ActivateNextPlayer()
		{
			// Vérifier s'il reste des joueurs Alive
			if (NumberPlayerAlive() > 1)
			{
				// Désactive tout le monde comme protection
				//GetActivePlayer().IsActive = false;
				DesactivateAllPlayer();

				_currentPlayerId++;
				if (_currentPlayerId >= _maxPlayer)
				{
					_currentPlayerId = 0;
					GetActivePlayer().IsActive = true;
					OnEveryonePlayed();
				}
				if (GetActivePlayer().IsAlive == false)
				{
					ActivateNextPlayer();
				}
				OnActivePlayerChange();
				return true;
			}
			else
			{
				return false;
			}
		}

		public Player GetActivePlayer()
		{
			return _playerList[_currentPlayerId];
		}
		
		public List<Player> AllPlayerList()
		{
			return _playerList;
		}

		public List<Player> AlivePlayerList()
		{
			List<Player> returnList = new List<Player>();
			foreach (Player player in _playerList)
			{
				if (player.IsAlive)
				{
					returnList.Add(player);
				}
			}
			return returnList;
		}

		public int NumberPlayerAlive()
		{
			int nbAlive = 0;
			foreach (var item in _playerList)
			{
				if (item.IsAlive)
				{
					nbAlive++;
				}
			}
			return nbAlive;
		}

		public void ResetToFirstActivePlayer()
		{
			//TODO Vérifier si Actif
			DesactivateAllPlayer();
			_currentPlayerId = 0;
			GetActivePlayer().IsActive = true;
			OnActivePlayerChange();
		}

		private void DesactivateAllPlayer()
		{
			foreach (var player in _playerList)
			{
				player.IsActive = false;
			}
		}

		public Player GetHighestBaconAliveNotActivePlayer()
		{
			Player returnPlayer = null;
			// Trouver le pointage le plus élévé
			int maxBP = 0;
			foreach (Player player in GetNotActiveAndAlivePlayerList())
			{
				if (player.BaconPoint > maxBP)
				{
					maxBP = player.BaconPoint;
				}
			}
			if (maxBP == 0)
			{
				return null;
			}

			// Si il y a une égalité
			List<Player> highestPlayers = new List<Player>();
			foreach (Player player in GetNotActiveAndAlivePlayerList())
			{
				if (player.BaconPoint == maxBP)
				{
					highestPlayers.Add(player);
				}
			}

			if (highestPlayers.Count > 1)
			{
			}
			return returnPlayer;
		}
 		public List<Player> GetNotActiveAndAlivePlayerList()
		{
			List<Player> players = new List<Player>();
			foreach (var player in _playerList)
			{
				if (player.IsAlive && !player.IsActive)
				{
					players.Add(player);
				}
			}
			return players;
		}

		#region Events
		
		public event EventHandler ActivePlayerChangedEvent;
		protected void OnActivePlayerChange()
		{
			EventHandler handler = ActivePlayerChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		public event EventHandler EveryonePlayedEvent;
		protected void OnEveryonePlayed()
		{
			EventHandler handler = EveryonePlayedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}
		#endregion
	}
}
