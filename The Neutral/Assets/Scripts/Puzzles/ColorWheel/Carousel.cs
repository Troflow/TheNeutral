using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Carousel class.
	/// Manages initialisation of ordering, rotation speed,
	/// and keeps track of puzzle completion
	/// </summary>
	public class Carousel : Puzzle {
		private Transform centre;
		private List<ColorWheel> allColorWheelsInSystem;
		private List<ColorWheel> haltedColorWheels;

		private bool correctlyOrdered = false;

		void Start ()
		{
			haltedColorWheels = new List<ColorWheel>();
			allColorWheelsInSystem = new List<ColorWheel>();
			initialiseAllColorWheels();
		}

		/// <summary>
		/// Adds the wheel to list of color wheels
		/// that have been halted
		/// </summary>
		/// <param name="pColorWheel">P color wheel.</param>
		public void addWheelToHaltedWheels(ColorWheel pColorWheel)
		{
			if (!haltedColorWheels.Contains(pColorWheel))
			{

				validateHaltedWheels(pColorWheel);

				if (haltedColorWheels.Count == allColorWheelsInSystem.Count)
				{
					puzzleCompleted();
				}
			}
		}

		/// <summary>
		/// Once the number of halted wheels exceeds 1, check to see the walls's
		/// index in the list matches with its haltOrder
		/// </summary>
		private bool validateHaltedWheels(ColorWheel pColorWheel)
		{
			haltedColorWheels.Add(pColorWheel);

			foreach (ColorWheel wheel in haltedColorWheels)
			{
				// If even one wheel is not halted in the right ordering
				// resume rotation of all color wheels
				if (wheel.getHaltOrder() != haltedColorWheels.IndexOf(wheel))
				{
					resumeAllWheelsRotation();
					return false;
				}
			}

			pColorWheel.setIsHalted(true);
			return true;
		}

		private void resumeAllWheelsRotation()
		{
			foreach (ColorWheel wheel in allColorWheelsInSystem)
			{
				wheel.setIsHalted(false);
			}

			haltedColorWheels.Clear();
		}

		private void initialiseAllColorWheels()
		{
			var rotationDirection = 1f;
			foreach (Transform child in transform)
			{
				if (child.name == "Centre")
				{
					continue;
				}

				rotationDirection *= -1f;

				var colorWheel = child.GetComponent<ColorWheel>();
				var colorWheelMural = child.Find("Mural").GetComponent<Mural>();

				colorWheel.setHaltOrder(colorWheelMural.getHeight());
				colorWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);

				allColorWheelsInSystem.Add(colorWheel);
			}
		}

		protected override void puzzleCompleted()
		{
			correctlyOrdered = true;
			throw new System.NotImplementedException ();
		}
	}
}