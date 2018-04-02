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
		protected bool isCompleted = false;
		protected bool isActivated = false;
		public abstract void addWheelToHaltedWheels(ColorWheel pColorWheel);

		protected void resumeAllWheelsRotation()
		{
			foreach (ColorWheel wheel in allColorWheels)
			{
				wheel.setIsHalted(false);
			}

			haltedColorWheels.Clear();
		}

		public void changeAllMuralAndRingStatesTo(bool newState)
		{
			foreach (ColorWheel wheel in allColorWheels)
			{
				wheel.setRingState(newState);
				wheel.setMuralState(newState);
			}
		}
	}
}