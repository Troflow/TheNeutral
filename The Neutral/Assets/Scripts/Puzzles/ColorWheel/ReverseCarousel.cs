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

        public void addNewlyColoredWheel(ColorlessWheel wheel)
        {
            nowColoredWheels.Add(wheel);

            if (allColorlessWheels.Count == nowColoredWheels.Count)
            {
                validateAllWheelColorations();
            }
        }

		private void validateAllWheelColorations()
		{
            // Loop through each wheel, making sure that none of its neighbouring
            // wheels has the same color as it
            // Then, make sure that the color wheel doesn't have the same coloring
            // As its Mural
            // If all checks pass, the puzzle is completed. Else, return
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
		}
	}
}