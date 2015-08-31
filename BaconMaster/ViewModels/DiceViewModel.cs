using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using BaconMaster.Models;

namespace BaconMaster.ViewModels
{
	public class DiceViewModel : PropertyChangedBase, IDisposable
	{
		public DiceViewModel(Dice model)
		{
			_model = model;
			_model.ResultChangedEvent += HandleResultChanged;
			_model.LockChangedEvent += HandleLockChanged;
		}

		public void Dispose()
		{
			// Désabonne
			_model.ResultChangedEvent -= HandleResultChanged;
			_model.LockChangedEvent -= HandleLockChanged;
		}

		private Dice _model;

		public int Result
		{
			get { return _model.Result; }
			set
			{
				_model.Result = value;
				NotifyOfPropertyChange(() => Result);
			}
		}

		public bool IsLocked
		{
			get { return _model.IsLocked; }
			set
			{
				_model.IsLocked = value;
				NotifyOfPropertyChange(() => IsLocked);
			}
		}

		#region Handles
		private void HandleResultChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => Result);
		}
		private void HandleLockChanged(object sender, EventArgs e)
		{
			NotifyOfPropertyChange(() => IsLocked);
		}

		#endregion
	}
}
