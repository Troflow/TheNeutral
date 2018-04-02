using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// TripletCarousel class.
	/// Manages logic of one of the three carousels contained in this type
	/// of ColorWheel.
	/// </summary>
	public class TripletCarousel : StandardCarousel {
		TripletCarouselManager tripletCarouselManager;


		public void activate()
		{
			tripletCarouselManager = transform.parent.GetComponent<TripletCarouselManager>();
			isActivated = true;
			allColorWheels = new List<ColorWheel>();
			foreach (Transform child in transform)
			{
				allColorWheels.Add(child.GetComponent<ColorWheel>());
			}
			changeAllMuralAndRingStatesTo(true);
			if (tripletCarouselManager.getSystemType() == ColorWheelSystemType.Global) return;

			// If the ColorWheelSystemType is Local,
			// the carousel will further intialise its properties (rather than the
			// TripletCarouselManager)
			initialiseAllColorWheels();
		}

		public void deactivate()
		{
			changeAllMuralAndRingStatesTo(false);
			foreach(ColorWheel wheel in allColorWheels)
			{
				wheel.setIsHalted(false);
			}
			haltedColorWheels = null;
			allColorWheels = null;
			isActivated = false;
			tripletCarouselManager = null;
		}

        /// <summary>
        /// Handles logic when a ColorWheel has been successfully halted.
		/// Logic differs based on whether the ColorWheelSystemType is Global or Local.
        /// </summary>
        /// <param name="pColorWheel"></param>
		public override void addWheelToHaltedWheels(ColorWheel pColorWheel)
		{
			// If systemType is Global, tripletCarouselManager will be in charge of managing halted ColorWheels instead
			if (tripletCarouselManager.getSystemType() == ColorWheelSystemType.Global)
			{
				tripletCarouselManager.addWheelToHaltedWheels(pColorWheel);
				return;
			}

			// Else, systemType is Local, and each Carousel does its halt ColorWheel check separately
			if (!haltedColorWheels.Contains(pColorWheel))
			{
				haltedColorWheels.Add(pColorWheel);

				// If a single wheel isn't halted in the right ordering
				// resume rotation of all color wheels
				foreach (ColorWheel wheel in haltedColorWheels)
				{
					if (wheel.getHaltOrder() != haltedColorWheels.IndexOf(wheel))
					{
						resumeAllWheelsRotation();
						return;
					}
				}

				// Else, halt the newly added wheel's rotation
				pColorWheel.setIsHalted(true);

				if (haltedColorWheels.Count == allColorWheels.Count) completed();
			}
		}

		/// <summary>
		/// Sets the rotation and halt order of all child ColorWheels.
		/// Must localise the halt orders to be capped by the number of Wheels within the Carousel
		/// in order to conform with how halt order will be verified
		/// </summary>
		void initialiseAllColorWheels()
		{
			haltedColorWheels = new List<ColorWheel>();

			var rotationDirection = 1f;
			var wheelsToLocalize = new List<ColorWheel>();
			foreach (Transform child in transform)
			{
				rotationDirection *= -1f;
                var colorWheel = child.GetComponent<ColorWheel>();
                var colorWheelMural = child.Find("Mural").GetComponent<Mural>();

				colorWheel.setHaltOrder(colorWheelMural.getHeight());
				colorWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);
			}

			localiseWheelHaltOrders();
		}

		/// <summary>
		/// In a Triplet ColorWheel System, halt orders can be either global or local based,
		/// on the ColorWheelSystemType.
		/// e.g. 9 Murals, have halt orders 0 through 8. And in a Global ColorWheelSystemType,
		/// these murals must be solved in that ordering in order to solve the puzzle.
		/// In a Local ColorWheelSystemType however, each TripletCarousel, will have
		/// its own halt ordering. Thus:
		/// Carousel A will have Murals [0,4,8],
		/// Carousel B will have Murals [2,6,7]
		/// Carousel C will have Murals [1,3,5]
		/// If we don't localise, then it will be impossible to solve any of the Carousel's locally
		/// thus, localiseWheelHaltOrders() turns each of the above arrays to: [0,1,2]
		/// for the respective carousel
		/// </summary>
		void localiseWheelHaltOrders()
		{
			// Sort all of this TripletCarousel's ColorWheels by their Global halt order
			// and then set their halt order to their index in the list
			// of ColorWheels for this TripletCarousel
			allColorWheels.Sort((x,y) => x.getHaltOrder().CompareTo(y.getHaltOrder()));
            var localisedHeight = 0;
            foreach (ColorWheel wheel in allColorWheels)
            {
                wheel.setHaltOrder(localisedHeight);
                localisedHeight ++;
            }
		}

		void rotateAllWheels()
		{
			foreach (ColorWheel wheel in allColorWheels)
			{
				wheel.rotate();
			}
		}

		void completed()
		{
			isCompleted = true;
			tripletCarouselManager.addCompletedCarousel();
			deactivate();
		}

		void Update()
		{
			if (!isCompleted && isActivated) rotateAllWheels();
		}
	}
}