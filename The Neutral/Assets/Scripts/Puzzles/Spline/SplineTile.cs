using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Class handling SplineTiles of the Spline puzzle.
	/// SplineTiles can have at most one child - a SplineBox.
	/// </summary>
	public class SplineTile : MonoBehaviour {
		[SerializeField]
		Spline spline;
		[SerializeField]
		SplineBox box;

		bool blinkLineOccupied;
		bool sightLineOccupied;
		bool persistentLineOccupied;

		// For Debugging
		Transform linePrefab;

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
				// Nothing happens if !spline.isActivated, or spline.currentSplineLine is null
				if (spline.getCurrentSplineLine() == null || !spline.getIsActivated()) return;

				// If SplineTile has no SplineBox associated with it:
				if (box == null) spline.addTileToCurrentLine(transform.position);

				// If SplineTile does have a SplineBox associated with it:
				else
				{
					// If box that is a Blockade, do nothing
					if (box.getType() == SplineBoxType.Blockade) return;

					// If box is not the destination for spline.currentSplineLine, do nothing
					if (!spline.checkIfIsDestination(box)) return;

					var playerState = pCollider.GetComponentInParent<PlayerState>();
					var playerActionState = playerState.getPlayerActionState();

					// If box is the destination of spline.currentSplineLine,
					// increment spline.completedSplineCount by one
					if (spline.checkIfIsDestination(box))
					{
						spline.splineLineCompleted();
					}

					// If playerActionState is Attacking, instantiate SplineLine object and
					// set spline.currentSplineLine to instantiated SplineLine
					else if (playerActionState == PlayerActionState.Attacking)
					{
						box.activate(linePrefab);
						spline.setCurrentSplineLine(box.getSplineLine());
						spline.addTileToCurrentLine(transform.position);
					}
				}
			}
		}
	}
}
