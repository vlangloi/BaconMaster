using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaconMaster.Models
{
	public class Player
	{
		public Player(string name, int initialHP, int initialBacon)
		{
			Name = name;
			HitPoint = initialHP;
			BaconPoint = initialBacon;
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		private int _hitPoint;
		public int HitPoint
		{
			get { return _hitPoint; }
			set 
			{
				if (value.Equals(_hitPoint)) return;
				_hitPoint = value;
				OnHitPointChange();

				if (_hitPoint <= 0)
				{
					IsAlive = false;
				} 
				else
				{
					IsAlive = true;
				}			
			}
		}
		private int _baconPoint;
		public int BaconPoint
		{
			get { return _baconPoint; }
			set 
			{
				if (value.Equals(_baconPoint)) return;
				_baconPoint = value; 
				OnBaconPointChange(); 
			}
		}
		private bool _isAlive = true;
		public bool IsAlive
		{
			get { return _isAlive; }
			set 
			{
				if (value.Equals(_isAlive)) return;
				_isAlive = value; 
				OnAliveChange(); 
			}
		}

		private bool _isActive = false;
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (value.Equals(_isActive)) return;
				_isActive = value;
				OnActiveChange();
			}
		}
		#region
		public event EventHandler HitPointChangedEvent;
		protected void OnHitPointChange()
		{
			EventHandler handler = HitPointChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}
		public event EventHandler BaconPointChangedEvent;
		protected void OnBaconPointChange()
		{
			EventHandler handler = BaconPointChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}
		public event EventHandler AliveChangedEvent;
		protected void OnAliveChange()
		{
			EventHandler handler = AliveChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		public event EventHandler ActiveChangedEvent;
		protected void OnActiveChange()
		{
			EventHandler handler = ActiveChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}
		
		#endregion
		
	}
}
