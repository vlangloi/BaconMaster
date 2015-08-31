using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using BaconMaster.Models;
using System.Windows.Media;

namespace BaconMaster.ViewModels
{
	public class PlayerViewModel : PropertyChangedBase, IDisposable
	{
		public PlayerViewModel(Player model)
		{
			_model = model;

			// Abonnement au changement des modèles
			_model.HitPointChangedEvent += HandleHitPointChanged;
			_model.BaconPointChangedEvent += HandleBaconPointChanged;
			_model.AliveChangedEvent += HandleAliveChanged;
			_model.ActiveChangedEvent += HandleActiveChanged;
		}

		public void Dispose()
		{
			// Désabonne
			_model.HitPointChangedEvent -= HandleHitPointChanged;
			_model.BaconPointChangedEvent -= HandleBaconPointChanged;
			_model.AliveChangedEvent -= HandleAliveChanged;
			_model.ActiveChangedEvent -= HandleActiveChanged;
		}

		private Player _model;

		public string Name
		{
			get { return _model.Name; }
			set { _model.Name = value; NotifyOfPropertyChange(() => Name); }
		}

		public int HitPoint
		{
			get { return _model.HitPoint; }
			set
			{
				_model.HitPoint = value;
				NotifyOfPropertyChange(() => HitPoint);
			}
		}

		public int BaconPoint
		{
			get { return _model.BaconPoint; }
			set
			{
				_model.BaconPoint = value;
				NotifyOfPropertyChange(() => BaconPoint);
			}
		}

		public bool IsActive
		{
			get { return _model.IsActive; }
// 			protected set
// 			{
// 				//_model.IsAlive = value;
// 				NotifyOfPropertyChange(() => IsActive);
// 				NotifyOfPropertyChange(() => StatusBackgroundColor);
// 			}
		}

		// Changer la couleur du background à rouge quand mort
		public bool IsAlive
		{
			get { return _model.IsAlive; }
// 			protected set
// 			{
// 				//_model.IsAlive = value;
// 				NotifyOfPropertyChange(() => IsAlive);
// 				NotifyOfPropertyChange(() => StatusBackgroundColor);
// 			}
		}

		public SolidColorBrush StatusBackgroundColor
		{
			get
			{
				if (_model.IsAlive)
				{
					return new SolidColorBrush(Colors.LightGreen);
				}
				else
				{
					return new SolidColorBrush(Colors.Red);
				}
			}
		}

		#region Handles
		private void HandleNameChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => Name);
		}
		private void HandleHitPointChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => HitPoint);
		}
		private void HandleBaconPointChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => BaconPoint);
		}
		private void HandleAliveChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => IsAlive);
			NotifyOfPropertyChange(() => StatusBackgroundColor);
		}
		private void HandleActiveChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => IsActive);
		}
		#endregion
	}
}
