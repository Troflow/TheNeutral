using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ReverseCarousel class.
	/// Manages initialisation of ordering, rotation speed,
	/// and keeps track of puzzle completion
	/// </summary>
	public class ReverseCarousel : MonoBehaviour, IInteractable {
		bool isSolved = false;
		bool isActivated = false;
        Transform lantern;
        List<ColorlessWheel> allColorlessWheels;
		List<ColorlessWheel> nowColoredWheels;

		public bool IsBeingInteractedWith { get; set; }

		public void Interact()
		{
			if (isSolved) return;

			if (isActivated) Deactivate();
			else Activate();
		}

		void Activate()
		{
			allColorlessWheels = new List<ColorlessWheel>();
            nowColoredWheels = new List<ColorlessWheel>();
            setLantern();
			initialiseAllChildren();
			changeAllMuralStatesTo(true);

			isActivated = true;
		}

		void Deactivate()
		{
			changeAllMuralStatesTo(false);
			clearAllColorlessRings();
			allColorlessWheels = null;
            nowColoredWheels = null;

			isActivated = false;
		}

        public bool addNewlyColoredWheel(Transform pWheel)
        {
			// Ignore all events if Deactivated or not yet solved
			if (!isActivated || isSolved) return false;

            nowColoredWheels.Add(pWheel.GetComponent<ColorlessWheel>());

            if (allColorlessWheels.Count == nowColoredWheels.Count)
            {
                validateAllWheelColorations();
            }

			return true;
        }

		/// <summary>
		/// Makes sure each ColorWheel in the ReverseCarousel is colored such that the puzzle may be solved.
		/// If a single check fails, the remaining validation will cease immediately.
		/// </summary>
		void validateAllWheelColorations()
		{
			// Loop through Wheels until the penultimate ColorWheel in allColorWheels
			var allColorsInSystem = new List<CombatColor>();
			for (int x = 0; x < allColorlessWheels.Count-1; x++)
			{
				var index = x;
				var currentWheel = allColorlessWheels[index];
				var nextWheel = allColorlessWheels[++index];

				if (!allColorsInSystem.Contains(currentWheel.getRingColor()))
				{
					allColorsInSystem.Add(currentWheel.getRingColor());
				}

				// TODO: Make the CombatColor Comparison deterministic. It doesn't work properly all the time
				var currentRingMatchesMural = currentWheel.getRingColor() == currentWheel.getMuralColor();
				var nextRingMatchesMural = nextWheel.getRingColor().color.Key == nextWheel.getMuralColor().color.Key;
				var currentRingMatchesNextRing = currentWheel.getRingColor() == nextWheel.getRingColor();
				var nextRingColorAlreadyUsed = allColorsInSystem.Contains(nextWheel.getRingColor());

				if (currentRingMatchesMural || nextRingMatchesMural || currentRingMatchesNextRing || nextRingColorAlreadyUsed)
				{
					return;
				}
			}
			puzzleCompleted();
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

				var colorlessWheel = child.GetComponent<ColorlessWheel>();
				colorlessWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);
				allColorlessWheels.Add(colorlessWheel);
			}
		}

		void changeAllMuralStatesTo(bool pNewState)
		{
			foreach (ColorlessWheel wheel in allColorlessWheels)
			{
				wheel.setMuralState(pNewState);
			}
		}

		void clearAllColorlessRings()
		{
			foreach (ColorlessWheel wheel in allColorlessWheels)
			{
				wheel.clearColorlessRingColor();
			}
		}

		void rotateAllWheels()
		{
			foreach (ColorlessWheel wheel in allColorlessWheels)
			{
				wheel.rotate();
			}
		}

		void puzzleCompleted()
		{
			isSolved = true;
			lantern.gameObject.SetActive(true);
			changeAllMuralStatesTo(false);
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