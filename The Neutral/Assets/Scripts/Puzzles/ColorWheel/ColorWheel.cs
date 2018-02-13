using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ColorWheel class.
	/// Responsible for rotating, and is the parent object of the ColorRing and Mural
	/// </summary>
	public class ColorWheel : MonoBehaviour
	{
		private Carousel carousel;

		private int haltOrder;
		private Vector3 rotateVector;
		private bool isHalted = false;

		void Start ()
		{
			carousel = transform.parent.GetComponent<Carousel>();
		}

		public void setRotateVector(float rotateSpeed)
		{
			rotateVector = new Vector3 (0, rotateSpeed, 0);
		}

		public void setHaltOrder(int newHaltOrder)
		{
			haltOrder = newHaltOrder;
		}

		public int getHaltOrder()
		{
			return haltOrder;
		}

		public void setIsHalted(bool newState)
		{
			isHalted = newState;
		}

		public void setMuralState(bool newState)
		{
			transform.Find("Mural").gameObject.SetActive(newState);
		}

		/// <summary>
		/// Called by a Mural when it has been successfully colored by the Player
		/// </summary>
		public void halt()
		{
			carousel.addWheelToHaltedWheels(this);
		}

		void Update ()
		{
			if (!isHalted)
			{
				transform.Rotate(rotateVector * Time.deltaTime);
			}
		}
	}
}
