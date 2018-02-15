using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// StandardCarousel class.
	/// Manages initialisation of ordering, rotation speed,
	/// and keeps track of puzzle completion
	/// </summary>
	public abstract class StandardCarousel : MonoBehaviour {

		protected List<ColorWheel> allColorWheels;
		protected List<ColorWheel> haltedColorWheels;
		protected bool isSolved = false;
		public abstract void addWheelToHaltedWheels(ColorWheel pColorWheel);

		public bool getIsSolved()
		{
			return isSolved;
		}

		protected void resumeAllWheelsRotation()
		{
			foreach (ColorWheel wheel in allColorWheels)
			{
				wheel.setIsHalted(false);
			}

			haltedColorWheels.Clear();
		}
	}
}