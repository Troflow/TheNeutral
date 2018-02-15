using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// SingleSystemCarousel class.
	/// Manages initialisation of ordering, rotation speed,
	/// and keeps track of puzzle completion
	/// </summary>
	public class SoloCarousel : StandardCarousel {
		private Transform lantern;

		void Start()
		{
			haltedColorWheels = new List<ColorWheel>();
			allColorWheels = new List<ColorWheel>();
            setLantern();
			initialiseAllChildren();
		}

		public override void addWheelToHaltedWheels(ColorWheel pColorWheel)
		{
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

				// If the number of halted wheels is the same number as all wheels in the system
				// set the puzzle as completed.
				if (haltedColorWheels.Count == allColorWheels.Count)
				{
					puzzleCompleted();
				}
			}
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
				if (child.name == "Centre") continue;

				var colorWheel = child.GetComponent<ColorWheel>();
				var colorWheelMural = child.Find("Mural").GetComponent<Mural>();

				colorWheel.setHaltOrder(colorWheelMural.getHeight());
				colorWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);

				allColorWheels.Add(colorWheel);
			}
		}

		private void puzzleCompleted()
		{
			isSolved = true;
			lantern.gameObject.SetActive(true);

			foreach (ColorWheel wheel in allColorWheels)
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