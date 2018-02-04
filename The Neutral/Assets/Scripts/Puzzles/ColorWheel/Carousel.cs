using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{


    //if experiencing odd behaviour with colliders it's okay for now!

	/// <summary>
	/// Class responsible for tracking ColorWheel puzzle completion
	/// </summary>
	public class Carousel : Puzzle {
		private Transform centre;
		private List<ColorWheel> haltedColorWheels;

		private bool correctlyOrdered;

		void Start () {
			haltedColorWheels = new List<ColorWheel> ();

		}

		/// <summary>
		/// Adds the wheel to list of color wheels
		/// that have been halted
		/// </summary>
		/// <param name="pColourWheel">P colour wheel.</param>
		public void addWheel(ColorWheel pColourWheel)
		{
			if (!haltedColorWheels.Contains (pColourWheel))
			{
				haltedColorWheels.Add (pColourWheel);
				validateHaltedWheels ();
			}
		}

		/// <summary>
		/// Once the number of halted wheels exceeds 2,
		/// will check to see the walls's index in the list
		/// matches with its haltOrder
		/// </summary>
		private void validateHaltedWheels()
		{
			if (haltedColorWheels.Count < 2) return;

			// Loop through haltedColourWheels, making sure each wheel's halt order
			// matches their index in the list
			foreach (ColorWheel wheel in haltedColorWheels)
			{
				if (wheel.haltOrder != haltedColorWheels.IndexOf (wheel))
				{
					correctlyOrdered = false;
					return;
				}
			}

			correctlyOrdered = true;
			puzzleCompleted ();
		}

		protected override void puzzleCompleted ()
		{
			throw new System.NotImplementedException ();
		}
	}
}