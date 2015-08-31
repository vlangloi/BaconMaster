using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaconMaster.Models
{
	public class Dice
	{
		public Dice(int side, Random rnd)
		{
			_diceSide = side;
			_rndSeed = rnd;
			RollDice();
		}

		private int _diceSide = 6;
		public int DiceSide
		{
			get { return _diceSide; }
			private set { _diceSide = value; }
		}

		private int _result = 1;
		public int Result
		{
			get { return _result; }
			set 
			{
				if (value.Equals(_result)) return;
				if (value < 1)
				{
					value = 1;
				}
				if (value > DiceSide)
				{
					value = DiceSide;
				}
				_result = value; 
				OnResultChange(); 
			}
		}

		private bool _isLocked = false;
		public bool IsLocked
		{
			get { return _isLocked; }
			set
			{
				if (value.Equals(_isLocked)) return;
				_isLocked = value;
				OnLockChange();
			}
		}
		private Random _rndSeed;
		public void RollDice()
		{
			//https://channel9.msdn.com/Forums/TechOff/7321-C-dice-Roller
			Result = _rndSeed.Next(1, 7);
		}

		public event EventHandler ResultChangedEvent;
		protected void OnResultChange()
		{
			EventHandler handler = ResultChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		public event EventHandler LockChangedEvent;
		protected void OnLockChange()
		{
			EventHandler handler = LockChangedEvent;
			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}
	}
}
