using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaconMaster.Models;

namespace BaconMaster
{
	public class DiceManager
	{
		public DiceManager(int numberOfDice)
		{
			_maxDiceNumber = numberOfDice;
			_rnd = new Random((Int32)DateTime.Now.Ticks);
			DiceArray = new Dice[numberOfDice];
			for (int i = 0; i < _maxDiceNumber; i++ )
			{
				DiceArray[i] = new Dice(6, _rnd);
			}
		}
		private Random _rnd;

		private Dice[] _diceArray;
		public Dice[] DiceArray
		{
			get { return _diceArray; }
			private set { _diceArray = value; }
		}

		private int _maxDiceNumber;
		public int MaxDiceNumber
		{
			get { return _maxDiceNumber; }
			private set { _maxDiceNumber = value; }
		}

		public void RollAll()
		{
			foreach (Dice item in DiceArray)
			{
				item.RollDice();
			}
		}

		public void RollUnlocked()
		{
			foreach (Dice item in DiceArray)
			{
				if (!item.IsLocked)
				{
					item.RollDice();
				}				
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Dice Result = ");
			foreach (Dice item in DiceArray)
			{
				sb.AppendFormat("{0}; ", item.Result);
			}
			return sb.ToString();
		}
	}
}
