using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ColorlessWheel class.
	/// Responsible for rotating, and is the parent object of the ColorlessRing and ReverseMural
	/// </summary>
	public class ColorlessWheel : MonoBehaviour
	{
		ReverseCarousel carousel;
		Vector3 rotateVector;

		void Start ()
		{
			carousel = transform.parent.GetComponent<ReverseCarousel>();
		}

		public CombatColor getRingColor()
		{
			return transform.Find("ColorlessRing").GetComponent<ColorlessRing>().getCombatColor();
		}

		public CombatColor getMuralColor()
		{
			return transform.Find("ReverseMural").GetComponent<ReverseMural>().getCombatColor();
		}

		public void setRotateVector(float pRotateSpeed)
		{
			rotateVector = new Vector3 (0, pRotateSpeed, 0);
		}

		public void setMuralState(bool pNewState)
		{
			var muralObject = transform.Find("ReverseMural").gameObject;
			muralObject.SetActive(pNewState);

			if (pNewState) muralObject.GetComponent<ReverseMural>().Activate();
			else muralObject.GetComponent<ReverseMural>().Deactivate();
		}

		public void clearColorlessRingColor()
		{
			transform.Find("ColorlessRing").GetComponent<ColorlessRing>().clearColoringBook();
		}

		public void rotate()
		{
			transform.Rotate(rotateVector * Time.deltaTime);
		}
	}
}