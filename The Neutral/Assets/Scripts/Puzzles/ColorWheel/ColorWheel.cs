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
		int haltOrder;
		Vector3 rotateVector;
		bool isHalted = false;

		public void setRotateVector(float pRotateSpeed)
		{
			rotateVector = new Vector3 (0, pRotateSpeed, 0);
		}

		public void setHaltOrder(int pNewHaltOrder)
		{
			haltOrder = pNewHaltOrder;
		}

		public int getHaltOrder()
		{
			return haltOrder;
		}

		public void setIsHalted(bool pNewState)
		{
			isHalted = pNewState;
		}

		public void setRingState(bool pNewState)
		{
			if (pNewState) transform.Find("Ring").GetComponent<ColorRing>().activate();
			else transform.Find("Ring").GetComponent<ColorRing>().deactivate();
		}

		public void setMuralState(bool pNewState)
		{
			var mural = transform.Find("Mural").gameObject;
			mural.SetActive(pNewState);

			if (pNewState) mural.GetComponent<Mural>().activate();
			else mural.GetComponent<Mural>().deactivate();

		}

		/// <summary>
		/// Called by a Mural when it has been successfully colored by the Player
		/// </summary>
		public void halt()
		{
			var carousel = transform.parent.GetComponent<StandardCarousel>();
			carousel.addWheelToHaltedWheels(this);
		}

		public void rotate()
		{
			if (!isHalted) transform.Rotate(rotateVector * Time.deltaTime);
		}
	}
}
