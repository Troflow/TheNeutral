﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Carousel class.
	/// Manages initialisation of ordering, rotation speed,
	/// and keeps track of puzzle completion
	/// </summary>
	public abstract class Carousel : MonoBehaviour {
		protected List<ColorWheel> allColorWheelsInSystem;
		protected List<ColorWheel> haltedColorWheels;

		protected bool isSolved = false;

		public abstract void addWheelToHaltedWheels(ColorWheel pColorWheel);

		public bool getIsSolved()
		{
			return isSolved;
		}

		protected void resumeAllWheelsRotation()
		{
			foreach (ColorWheel wheel in allColorWheelsInSystem)
			{
				wheel.setIsHalted(false);
			}

			haltedColorWheels.Clear();
		}
	}
}