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
	public class ReverseCarousel : MonoBehaviour {
		private bool isSolved = false;
        private Transform lantern;
        private List<ColorlessWheel> allColorlessWheels;
		private List<ColorlessWheel> nowColoredWheels;

		void Start()
		{
			allColorlessWheels = new List<ColorlessWheel>();
            nowColoredWheels = new List<ColorlessWheel>();
            setLantern();
			initialiseAllChildren();
		}

        public void addNewlyColoredWheel(Transform wheel)
        {
            nowColoredWheels.Add(wheel.GetComponent<ColorlessWheel>());

            if (allColorlessWheels.Count == nowColoredWheels.Count)
            {
                validateAllWheelColorations();
            }
        }

		private void validateAllWheelColorations()
		{
			// Loop through Wheels until the penultimate ColorWheel in allColorWheels
			List<CombatColor> allColorsInSystem = new List<CombatColor>();
			for (int x = 0; x < allColorlessWheels.Count-1; x++)
			{
				var index = x;
				var currentWheel = allColorlessWheels[index];
				var nextWheel = allColorlessWheels[++index];

				if (!allColorsInSystem.Contains(currentWheel.getRingColor()))
				{
					allColorsInSystem.Add(currentWheel.getRingColor());
				}

				var currentRingMatchesMural = currentWheel.getRingColor() == currentWheel.getMuralColor();
				var nextRingMatchesMural = nextWheel.getRingColor().color.Key == nextWheel.getMuralColor().color.Key;
				var currentRingMatchesNextRing = currentWheel.getRingColor() == nextWheel.getRingColor();
				var nextRingColorAlreadyUsed = allColorsInSystem.Contains(nextWheel.getRingColor());

				if (currentRingMatchesMural || nextRingMatchesMural || currentRingMatchesNextRing || nextRingColorAlreadyUsed)
				{
					// TODO: Make the CombatColor Comparison deterministic. It doesn't work properly all the time
					return;
				}
			}
			puzzleCompleted();
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

				var colorlessWheel = child.GetComponent<ColorlessWheel>();
				colorlessWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);
				allColorlessWheels.Add(colorlessWheel);
			}
		}

		private void rotateAllWheels()
		{
			foreach (ColorlessWheel wheel in allColorlessWheels)
			{
				wheel.rotate();
			}
		}

		private void puzzleCompleted()
		{
			isSolved = true;
			lantern.gameObject.SetActive(true);

			foreach (ColorlessWheel wheel in allColorlessWheels)
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

			if (!isSolved)
			{
				rotateAllWheels();
			}
		}
	}
}