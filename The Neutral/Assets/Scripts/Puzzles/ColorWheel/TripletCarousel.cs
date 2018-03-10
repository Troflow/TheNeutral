using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// TripletCarousel class.
	/// Manages initialisation of ordering, rotation speed,
	/// and keeps track of puzzle completion
	/// </summary>
	public class TripletCarousel : StandardCarousel {
		TripletCarouselManager tripletCarouselManager;
        Transform lantern;

		void Start()
		{
            tripletCarouselManager = transform.parent.GetComponent<TripletCarouselManager>();
		}

        public override void addWheelToHaltedWheels(ColorWheel pColorWheel)
		{
			// If systemType is Global, tripletCarouselManager is in charge of managing halted ColorWheels
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
		public void initialiseAllColorWheels()
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

		void localiseWheelHaltOrders()
		{
			allColorWheels.Sort((x,y) => x.getHaltOrder().CompareTo(y.getHaltOrder()));
            var localisedHeight = 0;
            foreach (ColorWheel wheel in allColorWheels)
            {
                wheel.setHaltOrder(localisedHeight);
                localisedHeight ++;
            }
		}


		public void Activate()
		{
			isActivated = true;
			allColorWheels = new List<ColorWheel>();
			foreach (Transform child in transform)
			{
				allColorWheels.Add(child.GetComponent<ColorWheel>());
			}
			changeAllMuralAndRingStatesTo(true);
			if (tripletCarouselManager.getSystemType() == ColorWheelSystemType.Global) return;

			// If TripletCarouselManager is Local System, then the carousel will further intialise its properties
			initialiseAllColorWheels();
		}

		public void Deactivate()
		{
			changeAllMuralAndRingStatesTo(false);
			foreach(ColorWheel wheel in allColorWheels)
			{
				wheel.setIsHalted(false);
			}
			haltedColorWheels = null;
			allColorWheels = null;
			isActivated = false;
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
			isSolved = true;
			tripletCarouselManager.addCompletedCarousel(this);
			Deactivate();
		}

		void Update()
		{
			if (!isSolved && isActivated) rotateAllWheels();
		}
	}
}