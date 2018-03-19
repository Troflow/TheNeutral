using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Neutral
{
	/// <summary>
	/// Class handling SplineTiles of the Spline puzzle.
	/// SplineTiles can have at most one child - a SplineBox.
	/// The naming convention for this Class is:
	/// "[Row_Index],[Column_Index]"
	/// e.g. "4,1"
	/// Of note- there is no " " after the comma. The left value is for the row
	/// and the right vaue is for the column.
	/// Failure to adhere to this scheme will affect the tileTooFarToAdd() method
	/// </summary>
	public class SplineTile : MonoBehaviour {

		static SplineTile tileMostRecentlyActivated;

		[SerializeField]
		Spline spline;
		[SerializeField]
		SplineBox box;
		SplineLine occupyingSplineLine;

		[SerializeField]
		bool sightLineOccupied;
		[SerializeField]
		bool blinkLineOccupied;
		[SerializeField]
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

		void handleOccupation(SplineLine pSplineLine)
		{
			clearOccupations();

			occupyingSplineLine = pSplineLine;
			occupyingSplineLine.addOccupiedTile(transform);

			var pSplineLineType = pSplineLine.getType();
			switch(pSplineLineType)
			{
				case SplineLineType.Sight:
					sightLineOccupied = true;
					break;
				case SplineLineType.Blinkable:
					blinkLineOccupied = true;
					break;
				case SplineLineType.Persistent:
					persistentLineOccupied = true;
					break;
			}
		}

		bool isVacantSplineTile(SplineLineType pSplineLineType)
		{
			// If SplineTile already has a persistentLine drawn on it - it is occupied
			if (persistentLineOccupied) return false;

			// If SplineTile already has sightLine drawn on it, and another sightLine
			// is incoming - it is occupied
			if (pSplineLineType == SplineLineType.Sight && sightLineOccupied) return false;

			// If SplineTile already has blinkLine drawn on it, and another blinkLine
			// is incoming - it is occupied
			if (pSplineLineType == SplineLineType.Blinkable && blinkLineOccupied) return false;

			return true;
		}

		bool tileTooFarToAdd()
		{
			// A tile is too far to add if its coords (discenred by its name) are greater
			// than plus or minus 1 for either X or Y position of the last SplineTile to be
			// added to the spline's currentSplineLine
			string[] lastAddedTileCoords = spline.getCurrentSplineLine().getLastOccupiedTile().name.Split(',');
			string[] currentTileCoords = this.name.Split(',');

			int lastAddedRow = Int32.Parse(lastAddedTileCoords[0]);
			int lastAddedColumn = Int32.Parse(lastAddedTileCoords[1]);

			int currentTileRow = Int32.Parse(currentTileCoords[0]);
			int currentTileColumn = Int32.Parse(currentTileCoords[1]);

			bool rowTooFar = Math.Abs(currentTileRow - lastAddedRow) > 1;
			bool columnTooFar = Math.Abs(currentTileColumn - lastAddedColumn) > 1;

			if (rowTooFar || columnTooFar) return true;
			else return false;
		}

		public void clearOccupations()
		{
			sightLineOccupied = false;
			blinkLineOccupied = false;
			persistentLineOccupied = false;

			occupyingSplineLine = null;
		}

		public void OnTriggerEnter(Collider pCollider)
		{
			if (pCollider.CompareTag("Player-Sphere"))
			{
				// Nothing happens if !spline.isActivated, or spline.currentSplineLine is null
				if (spline.getCurrentSplineLine() == null || !spline.getIsActivated()) return;

				// Check to prevent adding tiles too far apart (allows diagonal connections)
				if (tileTooFarToAdd()) return;

				// If an attempt is made to draw one SplineLine over another conflicting
				// SplineLine, wipe the line currently being drawn
				// Else, do nothing
				if (!isVacantSplineTile(spline.getCurrentSplineLineType()))
				{
					if (spline.getCurrentSplineLine() != occupyingSplineLine)
					{
						spline.clearCurrentSplineLine();
					}

					return;
				}

				// If SplineTile has no SplineBox associated with it:
				if (box == null)
				{
					spline.addTileToCurrentLine(this.name, transform.position);
					handleOccupation(spline.getCurrentSplineLine());
				}

				// If SplineTile does have a SplineBox associated with it:
				else
				{
					// If box that is a Blockade, do nothing
					if (box.getType() == SplineBoxType.Blockade) return;

					// If box is not the destination for spline.currentSplineLine, do nothing
					if (!spline.checkIfIsDestination(box)) return;

					// If box is the destination of spline.currentSplineLine,
					// increment spline.completedSplineCount by one
					if (spline.checkIfIsDestination(box))
					{
						spline.addTileToCurrentLine(this.name, transform.position);
						handleOccupation(spline.getCurrentSplineLine());

						spline.splineLineCompleted(box);
					}
				}
			}
		}

		public void OnTriggerStay(Collider pCollider)
		{
			if (pCollider.CompareTag("Player-Sphere"))
			{
				// Nothing happens if !spline.isActivated
				if (!spline.getIsActivated()) return;

				var playerState = pCollider.GetComponentInParent<PlayerState>();
				var playerActionState = playerState.getPlayerActionState();

				// If playerActionState is Attacking, instantiate SplineLine object and
				// set spline.currentSplineLine to instantiated SplineLine
				if (box != null && playerActionState == PlayerActionState.Attacking && tileMostRecentlyActivated != this)
				{
					box.activate(linePrefab, spline);
					spline.setCurrentSplineLine(box.getSplineLine());

					spline.addTileToCurrentLine(this.name, transform.position);
					handleOccupation(spline.getCurrentSplineLine());

					tileMostRecentlyActivated = this;
				}
				else if (box != null && playerActionState == PlayerActionState.Attacking && tileMostRecentlyActivated == this)
				{
					// print("Currently impossible to interact with the last activated SplineBox.");
				}
			}
		}
	}
}
