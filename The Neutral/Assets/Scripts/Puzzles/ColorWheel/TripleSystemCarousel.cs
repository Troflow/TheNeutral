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
	public class TripleSystemCarousel : Carousel {
		private TripleSystemManager systemManager;
        private Transform lantern;

		void Start()
		{
            systemManager = transform.parent.GetComponent<TripleSystemManager>();
			haltedColorWheels = new List<ColorWheel>();
			allColorWheelsInSystem = new List<ColorWheel>();
		}

        public override void addWheelToHaltedWheels(ColorWheel pColorWheel)
		{
			systemManager.addWheelToHaltedWheels();
		}
	}
}