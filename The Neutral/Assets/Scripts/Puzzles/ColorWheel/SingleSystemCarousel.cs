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
	public class SingleSystemCarousel : Carousel {
		private Transform lantern;

		void Start()
		{
			haltedColorWheels = new List<ColorWheel>();
			allColorWheelsInSystem = new List<ColorWheel>();
            setLantern();
			initialiseAllChildren();
		}

		public override void addWheelToHaltedWheels(ColorWheel pColorWheel)
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

        private void setLantern()
        {
            lantern = transform.Find("Centre").Find("Lantern");
            lantern.gameObject.SetActive(false);
        }

        private void initialiseAllChildren()
		{
			var rotationDirection = 1f;
			foreach (Transform child in transform)
			{
				rotationDirection *= -1f;
				if (child.name == "Centre")
				{
					continue;
				}

				var colorWheel = child.GetComponent<ColorWheel>();
				var colorWheelMural = child.Find("Mural").GetComponent<Mural>();

				colorWheel.setHaltOrder(colorWheelMural.getHeight());
				colorWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);

				allColorWheelsInSystem.Add(colorWheel);
			}
		}

		private void puzzleCompleted()
		{
			isSolved = true;
			lantern.gameObject.SetActive(true);

			foreach (ColorWheel wheel in allColorWheelsInSystem)
			{
				wheel.setMuralState(false);
			}
		}

		void Update()
		{
			// For Debugging
			if (lantern.gameObject.activeSelf)
			{
				lantern.Rotate(Vector3.up * 50f * Time.deltaTime);
			}
		}
	}
}