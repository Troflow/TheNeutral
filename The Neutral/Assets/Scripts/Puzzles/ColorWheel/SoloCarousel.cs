using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// SoloCarousel class.
	/// Manages initialisation of ordering, rotation speed,
	/// and keeps track of puzzle completion
	/// </summary>
	public class SoloCarousel : StandardCarousel, IInteractable {
		Transform lantern;
		public bool IsBeingInteractedWith { get; set; }

		public void Interact()
		{
			if (isSolved) return;

			if (isActivated) Deactivate();
			else Activate();
		}

		void Activate()
		{
			haltedColorWheels = new List<ColorWheel>();
			allColorWheels = new List<ColorWheel>();
            setLantern();
			initialiseAllChildren();
			changeAllMuralAndRingStatesTo(true);

			isActivated = true;
		}

		void Deactivate()
		{
			changeAllMuralAndRingStatesTo(false);
			haltedColorWheels = null;
			allColorWheels = null;

			isActivated = false;
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
				if (haltedColorWheels.Count == allColorWheels.Count) puzzleCompleted();
			}
		}

        void setLantern()
        {
            lantern = transform.Find("Centre").Find("Lantern");
            lantern.gameObject.SetActive(false);
        }

        void initialiseAllChildren()
		{
			var rotationDirection = 1f;
			foreach (Transform child in transform)
			{
				rotationDirection *= -1f;
				if (child.name == "Centre") continue;

				var colorWheel = child.GetComponent<ColorWheel>();
				var colorWheelMural = child.Find("Mural").GetComponent<Mural>();
				var colorWheelRing = child.Find("Ring").GetComponent<ColorRing>();

				colorWheel.setHaltOrder(colorWheelMural.getHeight());
				colorWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);
				colorWheel.setWillGrantColor(true);

				allColorWheels.Add(colorWheel);
			}
		}

		void rotateAllWheels()
		{
			foreach (ColorWheel wheel in allColorWheels)
			{
				wheel.rotate();
			}
		}

		void puzzleCompleted()
		{
			isSolved = true;
			lantern.gameObject.SetActive(true);
			changeAllMuralAndRingStatesTo(false);
		}

		void Update()
		{
			// For Debugging
			if (isActivated && lantern.gameObject.activeSelf)
			{
				lantern.Rotate(Vector3.up * 50f * Time.deltaTime);
			}

			if (!isSolved && isActivated) rotateAllWheels();
		}
	}
}