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
		bool isCompleted = false;
		bool isActivated = false;
        Transform lantern;
		List<Color> allMuralColors;
        List<ColorlessWheel> allColorlessWheels;
		List<ColorlessWheel> nowColoredWheels;

		public bool IsBeingInteractedWith { get; set; }

		public void Interact()
		{
			if (isCompleted) return;

			if (isActivated) deactivate();
			else activate();
		}

		void activate()
		{
			allColorlessWheels = new List<ColorlessWheel>();
            nowColoredWheels = new List<ColorlessWheel>();
			allMuralColors = new List<Color>();
            setLantern();
			initialiseAllChildren();
			changeAllMuralStatesTo(true);

			populateAllMuralColors();

			isActivated = true;
		}

		void deactivate()
		{
			changeAllMuralStatesTo(false);
			deactivateAllWheels();
			allColorlessWheels = null;
            nowColoredWheels = null;
			allMuralColors = null;

			isActivated = false;
		}

        public void addNewlyColoredWheel(Transform pWheel)
        {
			// Ignore all events if Deactivated or solved already
			if (!isActivated || isCompleted) return;

            var newlyColoredWheel = pWheel.GetComponent<ColorlessWheel>();
			if (!nowColoredWheels.Contains(newlyColoredWheel))
			{
				nowColoredWheels.Add(newlyColoredWheel);
			}

            if (allColorlessWheels.Count == nowColoredWheels.Count)
            {
                validateAllWheelColorations();
            }
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

				// Ring color can't match its Mural color.
				var currentRingMatchesMural = currentWheel.getRingColor() == currentWheel.getMuralColor();
				var nextRingMatchesMural = nextWheel.getRingColor().color.Key == nextWheel.getMuralColor().color.Key;

				// Ring color must be unique.
				var currentRingMatchesNextRing = currentWheel.getRingColor() == nextWheel.getRingColor();
				var nextRingColorAlreadyUsed = allColorsInSystem.Contains(nextWheel.getRingColor());

				// Ring color must match one of the Mural colors
				var currentRingColorIsNotValid = !allMuralColors.Contains(currentWheel.getRingColor().color.Value);
				var nextRingColorIsNotValid = !allMuralColors.Contains(nextWheel.getRingColor().color.Value);

				if (currentRingMatchesMural || nextRingMatchesMural ||
					currentRingMatchesNextRing || nextRingColorAlreadyUsed ||
					currentRingColorIsNotValid || nextRingColorIsNotValid)
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
				colorlessWheel.activate(rotationDirection);
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

		void populateAllMuralColors()
		{
			foreach (ColorlessWheel wheel in allColorlessWheels)
			{
				allMuralColors.Add(wheel.getMuralColor().color.Value);
			}
		}

		void deactivateAllWheels()
		{
			foreach (ColorlessWheel wheel in allColorlessWheels)
			{
				wheel.deactivate();
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
			isCompleted = true;
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

			if (!isCompleted && isActivated) rotateAllWheels();
		}
	}
}