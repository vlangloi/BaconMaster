using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using BaconMaster.Models;

namespace BaconMaster.ViewModels
{
	public class AllPlayerViewModel : PropertyChangedBase, IDisposable
	{
		public AllPlayerViewModel(List<Player> playerList)
		{
			Players = new BindableCollection<PlayerViewModel>();
			foreach (Player item in playerList)
			{
				Players.Add(new PlayerViewModel(item));
			}
			
		}

		public BindableCollection<PlayerViewModel> Players { get; private set; }

		public void Dispose()
		{
			foreach (var item in Players)
			{
				item.Dispose();
			}
		}
	}
}
