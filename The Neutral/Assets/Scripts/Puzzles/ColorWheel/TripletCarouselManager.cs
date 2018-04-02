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

        bool isCompleted = false;
        bool isActivated = false;

        Transform lantern;
        List<ColorWheel> globalHaltedColorWheels;
        List<ColorWheel> globalAllColorWheels;

        // Used by a Local ColorWheelSystem to know when all
        // Carousels have been successfully completed
        const int allCarouselsInSystemCount = 3;
        int completedCarouselsCount;

        public bool IsBeingInteractedWith { get; set; }

        public void Interact()
		{
			if (isCompleted) return;

			if (isActivated) deactivate();
			else activate();
		}

		void activate()
		{
            if (systemType == ColorWheelSystemType.Global)
            {
                globalHaltedColorWheels = new List<ColorWheel>();
                globalAllColorWheels = new List<ColorWheel>();
                initialiseAllCarousels();
            }

            setLantern();
			changeAllChildStatesInSystemTo(true);

			isActivated = true;
		}

		void deactivate()
		{
            changeAllChildStatesInSystemTo(false);

            if (systemType == ColorWheelSystemType.Global)
            {
                globalHaltedColorWheels = null;
                globalAllColorWheels = null;
            }

			isActivated = false;
		}

        /// <summary>
        /// If systemType is Global, sets up halt order and rotation vector for
        /// every wheel in every carousel of the system.
        /// </summary>
        void initialiseAllCarousels()
        {
            foreach (Transform child in transform)
			{
				if (child.name == "Centre") continue;

                initialiseColorWheelsForCarousel(child);
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

        /// <summary>
        /// Called by a TripletCarousel of a Local ColorWheelSystemType when all its color
        /// wheels have been successfully colored in the correct ordering
        /// </summary>
        /// <param name="carousel"></param>
        public void addCompletedCarousel()
        {
            completedCarouselsCount++;
            if (completedCarouselsCount == allCarouselsInSystemCount)
            {
                puzzleCompleted();
            }

        }

        /// <summary>
        /// This method is called by a child TripletCarousel in a Global ColorWheelSystemType
        ///  whenever one of its Murals is colored correctly
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

        void setLantern()
        {
            lantern = transform.Find("Centre").Find("Lantern");
            lantern.gameObject.SetActive(false);
        }

        /// <summary>
        /// In a Global ColorWheelSystem, if the ordering of any one halted wheel is incorrect,
        /// then every wheel is told to start rotating again.
        /// </summary>
        void globalResumeAllWheelsRotation()
        {
            foreach (ColorWheel wheel in globalAllColorWheels)
            {
                wheel.setIsHalted(false);
            }
        }

        /// <summary>
        /// Used to activate or deactivate all child TripletCarousels of this manager
        /// </summary>
        /// <param name="newState"></param>
        void changeAllChildStatesInSystemTo(bool newState)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Centre") continue;

                var carousel = child.GetComponent<TripletCarousel>();
                if (newState) carousel.activate();
                else carousel.deactivate();
            }
        }

        void puzzleCompleted()
		{
            if (systemType == ColorWheelSystemType.Global) changeAllChildStatesInSystemTo(false);
            lantern.gameObject.SetActive(true);
            isCompleted = true;
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