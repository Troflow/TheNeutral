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
	public class TripletCarouselManager : MonoBehaviour, IInteractable {
        [SerializeField]
        ColorWheelSystemType systemType;

        bool isSolved = false;
        bool isActivated = false;

        Transform lantern;
        List<ColorWheel> globalHaltedColorWheels;
        List<ColorWheel> globalAllColorWheels;
        List<TripletCarousel> allCarouselsInSystem;
        List<TripletCarousel> completedCarousels;

        public bool IsBeingInteractedWith { get; set; }

        public void Interact()
		{
			if (isSolved) return;

			if (isActivated) Deactivate();
			else Activate();
		}

		void Activate()
		{
            if (systemType == ColorWheelSystemType.Local)
            {
                completedCarousels = new List<TripletCarousel>();
                allCarouselsInSystem = new List<TripletCarousel>();
            }
            else if (systemType == ColorWheelSystemType.Global)
            {
                globalHaltedColorWheels = new List<ColorWheel>();
                globalAllColorWheels = new List<ColorWheel>();
            }

            setLantern();
			initialiseAllCarousels();
			changeAllChildStatesInSystemTo(true);

			isActivated = true;
		}

		void Deactivate()
		{
			changeAllChildStatesInSystemTo(false);

            if (systemType == ColorWheelSystemType.Local)
            {
                completedCarousels = null;
                allCarouselsInSystem = null;
            }
            else if (systemType == ColorWheelSystemType.Global)
            {
                globalHaltedColorWheels = null;
                globalAllColorWheels = null;
            }

			isActivated = false;
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
                    allCarouselsInSystem.Add(carousel);
                }
			}
        }

        /// <summary>
        /// Sets the rotation direction and halt ordering for the given carousel
        /// </summary>
        /// <param name="carousel"></param>
        void initialiseColorWheelsForCarousel(Transform carousel)
        {
            var rotationDirection = 1f;
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

        void setLantern()
        {
            lantern = transform.Find("Centre").Find("Lantern");
            lantern.gameObject.SetActive(false);
        }

        void globalResumeAllWheelsRotation()
        {
            foreach (ColorWheel wheel in globalAllColorWheels)
            {
                wheel.setIsHalted(false);
            }
        }

        void changeAllChildStatesInSystemTo(bool newState)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Centre") continue;

                var carousel = child.GetComponent<TripletCarousel>();
                if (newState) carousel.Activate();
                else carousel.Deactivate();
            }
        }

        void puzzleCompleted()
		{
            if (systemType == ColorWheelSystemType.Global) changeAllChildStatesInSystemTo(false);
            lantern.gameObject.SetActive(true);
            isSolved = true;
		}

        void Update()
		{
			// For Debugging
			if (isActivated && lantern.gameObject.activeSelf)
			{
				lantern.Rotate(Vector3.up * 50f * Time.deltaTime);
			}
		}
    }
}