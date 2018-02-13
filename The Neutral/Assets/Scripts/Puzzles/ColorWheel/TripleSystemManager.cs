using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// TripleSystemManager class.
	/// Manages TripleSystem ColorWheel Puzzles
	/// and the logic of both Global and Local Variations
	/// </summary>
	public class TripleSystemManager : MonoBehaviour {
        [SerializeField]
        private ColorWheelSystemType colorWheelType;

        private Transform lantern;
        private List<TripleSystemCarousel> allCarouselsInSystem;
        private List<TripleSystemCarousel> solvedCarousels;

        void Start()
        {
            solvedCarousels = new List<TripleSystemCarousel>();
            allCarouselsInSystem = new List<TripleSystemCarousel>();
            setLantern();
            initialiseAllChildren();
        }

        void initialiseAllChildren()
        {
            foreach (Transform child in transform)
			{
				if (child.name == "Centre")
				{
					lantern = child.Find("Lantern");
					lantern.gameObject.SetActive(false);
					continue;
				}

				var carousel = child.GetComponent<TripleSystemCarousel>();
				allCarouselsInSystem.Add(carousel);
			}
        }

        public void addWheelToHaltedWheels()
        {

        }

        private void setLantern()
        {
            lantern = transform.Find("Centre").Find("Lantern");
            lantern.gameObject.SetActive(false);
        }

        private void puzzleCompleted()
		{
            return;
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