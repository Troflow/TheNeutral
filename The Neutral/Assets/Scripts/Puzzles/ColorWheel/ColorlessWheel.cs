using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ColorlessWheel class.
	/// Responsible for rotating, and is the parent object of the ColorlessRing and ReverseMural.
	/// Ensure all Murals and Rings have the correct Lite's associated to them in the Inspector
	/// </summary>
	public class ColorlessWheel : MonoBehaviour
	{
		Vector3 rotateVector;

		public void activate(float pRotationDirection)
		{
			var rotateSpeed = GameManager.colorWheelRotateSpeed * pRotationDirection;
			rotateVector = new Vector3(0, rotateSpeed, 0);
		}

		public void deactivate()
		{
			transform.Find("ColorlessRing").GetComponent<ColorlessRing>().clearColoringBook();
		}

		public CombatColor getRingColor()
		{
			return transform.Find("ColorlessRing").GetComponent<ColorlessRing>().getCombatColor();
		}

		public CombatColor getMuralColor()
		{
			return transform.Find("ReverseMural").GetComponent<ReverseMural>().getCombatColor();
		}

		public void setMuralState(bool pNewState)
		{
			var muralObject = transform.Find("ReverseMural").gameObject;
			muralObject.SetActive(pNewState);

			if (pNewState) muralObject.GetComponent<ReverseMural>().activate();
			else muralObject.GetComponent<ReverseMural>().deactivate();
		}

		public void rotate()
		{
			transform.Rotate(rotateVector * Time.deltaTime);
		}
	}
}