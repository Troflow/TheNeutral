using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class SplineTile : MonoBehaviour {

		public bool isAware;

		[SerializeField]
		private Spline spline;
		[SerializeField]
		private SplineBox box;

		private bool blinkLineOccupied;
		private bool sightLineOccupied;
		private bool persistentLineOccupied;

		// For Debugging
		private Transform linePrefab;

		public void activate(Transform pLinePrefab)
		{
			if (box != null) linePrefab = pLinePrefab;
		}

		public void setSpline(Spline pSpline)
		{
			spline = pSpline;
		}

		public void setSplineBox(SplineBox pBox)
		{
			box = pBox;
		}

		public void clearAllAttributes()
		{
			// To be called when Spline is deactivated. Set everything to null (recursively)
		}

		public void OnTriggerEnter(Collider pCollider)
		{
			if (pCollider.CompareTag("Player-Sphere"))
			{
				if (box != null && linePrefab != null)
				{

					box.activate(linePrefab);
					var splineLine = box.getSplineLine();
					spline.setCurrentLineObject(splineLine);
				}

				spline.addTileToCurrentLine(transform.position);

			}
		}
	}
}
