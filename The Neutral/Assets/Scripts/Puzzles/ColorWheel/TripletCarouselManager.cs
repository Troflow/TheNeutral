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
	public class TripletCarouselManager : MonoBehaviour {
        [SerializeField]
        private ColorWheelSystemType systemType;

        private bool isSolved = false;
        private Transform lantern;
        private List<ColorWheel> globalHaltedColorWheels;
        private List<ColorWheel> globalAllColorWheels;
        private List<TripletCarousel> allCarouselsInSystem;
        private List<TripletCarousel> completedCarousels;

        void Start()
        {
            completedCarousels = new List<TripletCarousel>();
            allCarouselsInSystem = new List<TripletCarousel>();
            if (systemType == ColorWheelSystemType.Global)
            {
                globalHaltedColorWheels = new List<ColorWheel>();
                globalAllColorWheels = new List<ColorWheel>();
            }
            setLantern();
            initialiseAllCarousels();
        }

        void initialiseAllCarousels()
        {
            foreach (Transform child in transform)
			{
				if (child.name == "Centre")
				{
					lantern = child.Find("Lantern");
					lantern.gameObject.SetActive(false);
					continue;
				}

				var carousel = child.GetComponent<TripletCarousel>();

                // If systemType is Global, TripleSystemManager will handle all initiliasations
                if (systemType == ColorWheelSystemType.Global)
                {
                    initialiseColorWheelsForCarousel(carousel.transform);
                }
                // Else if systemType is Local, then each Carousel handles its own initialisation
                else if (systemType == ColorWheelSystemType.Local)
                {
                    carousel.initialiseAllColorWheels();
                }

                allCarouselsInSystem.Add(carousel);
			}
        }

        /// <summary>
        /// Sets the rotation direction and halt ordering for the given carousel
        /// </summary>
        /// <param name="carousel"></param>
        void initialiseColorWheelsForCarousel(Transform carousel)
        {
            var rotationDirection = 1f;
            var wheelsToLocalize = new List<ColorWheel>();
            foreach (Transform child in carousel)
            {
                rotationDirection *= -1f;
                var colorWheel = child.GetComponent<ColorWheel>();
                var colorWheelMural = child.Find("Mural").GetComponent<Mural>();

                colorWheel.setHaltOrder(colorWheelMural.getHeight());
                colorWheel.setRotateVector(GameManager.colorWheelRotateSpeed * rotationDirection);
                globalAllColorWheels.Add(colorWheel);
            }
        }

        public void addCompletedCarousel(TripletCarousel carousel)
        {
            completedCarousels.Add(carousel);
            if (completedCarousels.Count == allCarouselsInSystem.Count)
            {
                puzzleCompleted();
            }

        }

        /// <summary>
        /// This method is called by a child TripletCarousel whenever one of its Murals is colored correctly
        /// </summary>
        /// <param name="pColorWheel"></param>
        public void addWheelToHaltedWheels(ColorWheel pColorWheel)
        {
            if (!globalHaltedColorWheels.Contains(pColorWheel))
            {
                globalHaltedColorWheels.Add(pColorWheel);

                foreach (ColorWheel wheel in globalHaltedColorWheels)
                {
                    if (wheel.getHaltOrder() != globalHaltedColorWheels.IndexOf(wheel))
                    {
                        globalResumeAllWheelsRotation();
                        return;
                    }
                }

                pColorWheel.setIsHalted(true);
                if (globalHaltedColorWheels.Count == globalAllColorWheels.Count) puzzleCompleted();
            }
        }

        public ColorWheelSystemType getSystemType()
        {
            return systemType;
        }

        public bool getIsSolved()
        {
            return isSolved;
        }

        private void setLantern()
        {
            lantern = transform.Find("Centre").Find("Lantern");
            lantern.gameObject.SetActive(false);
        }

        private void globalResumeAllWheelsRotation()
        {
            foreach (ColorWheel wheel in globalAllColorWheels)
            {
                wheel.setIsHalted(false);
            }
        }
        private void deactivateAllMurals()
        {
            foreach (ColorWheel wheel in globalAllColorWheels)
            {
                wheel.setMuralState(false);
            }
        }

        private void puzzleCompleted()
		{
            if (systemType == ColorWheelSystemType.Global) deactivateAllMurals();
            lantern.gameObject.SetActive(true);
            isSolved = true;
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