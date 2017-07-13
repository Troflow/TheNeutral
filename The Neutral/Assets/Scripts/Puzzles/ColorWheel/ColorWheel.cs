using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Color wheel class, responsible for spinning and
	/// halting when child Color wall has been properly colored.
	/// </summary>
	public class ColorWheel : MonoBehaviour {

		private Carousel carousel;

		public int haltOrder;
		public bool isHalted = false;

		private Vector3 spinVector;
		private float spinSpeed = 10f;
		private bool isClockwise = true;

		void Start () {
			carousel = transform.parent.GetComponent<Carousel> ();

			if (!isClockwise) 
			{
				spinSpeed *= -1f;
			}
			spinVector = new Vector3 (0, spinSpeed, 0);
		}

		public void halt()
		{
			isHalted = true;
			carousel.addWheel (this);
		}

		public void spin()
		{
			if (!isHalted) 
			{
				transform.Rotate (spinVector * Time.deltaTime);
			}
		}
			
		void Update () {
			spin ();
		}
	}
}
