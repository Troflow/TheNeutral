using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ColorlessWheel class.
	/// Responsible for rotating, and is the parent object of the ColorRing and Mural
	/// </summary>
	public class ColorlessWheel : MonoBehaviour
	{

		private ReverseCarousel carousel;
		private Vector3 rotateVector;

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

		public void setRotateVector(float rotateSpeed)
		{
			rotateVector = new Vector3 (0, rotateSpeed, 0);
		}

		public void setMuralState(bool newState)
		{
			transform.Find("ReverseMural").gameObject.SetActive(newState);
		}

		public void rotate()
		{
			transform.Rotate(rotateVector * Time.deltaTime);
		}
	}
}