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

        public void getNewColor(CombatColor color)
        {
            carousel.addNewlyColoredWheel(this);
        }

		public void setRotateVector(float rotateSpeed)
		{
			rotateVector = new Vector3 (0, rotateSpeed, 0);
		}

		public void setMuralState(bool newState)
		{
			transform.Find("Mural").gameObject.SetActive(newState);
		}

		void Update ()
		{
			transform.Rotate(rotateVector * Time.deltaTime);
		}
	}
}
