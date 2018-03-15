
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class Spline : MonoBehaviour {

        // TODO: Remove all the private modifiers

		private List<Transform> tiles;

        private float splineLineOffset = 3.5f;
		private bool isActivated = false;
		private SplineLine currentSplineLine;
		private List<Vector3> currentLinePoints;

		private int splineLineCount;
		private int completedSplineLineCount = 0;

		// For Debugging
		public Transform linePrefab;

		void Start () {
			tiles = new List<Transform>();
			populateTiles ();
			currentLinePoints = new List<Vector3>();
		}

		void activateTileField ()
		{
			isActivated = true;
			foreach (Transform tile in tiles)
			{
				tile.GetComponent<SplineTile>().activate(linePrefab);
			}
		}

		void deactivateTileField()
		{
			isActivated = false;
		}

		public void setCurrentLineObject(SplineLine pLine)
		{
			if (!isActivated)
				return;

			// Clear the previous SplineLine of all its points if it wasn't completed
			if (!currentSplineLine == null) currentSplineLine.removeAllPoints();
			currentSplineLine = pLine;
		}

		public void addTileToCurrentLine(Vector3 pTilePos)
		{
			if (!isActivated || currentSplineLine == null)
				return;


			var tilePos = new Vector3();
			if (!currentLinePoints.Contains(pTilePos))
			{
				tilePos = pTilePos;
				tilePos.y += splineLineOffset;
				currentLinePoints.Add(tilePos);

				updateLineRendererPositions(currentSplineLine.getLineRenderer());
			}
		}

		public void updateLineRendererPositions(LineRenderer pLineRenderer)
		{
			pLineRenderer.positionCount = currentLinePoints.Count;
			pLineRenderer.SetPositions(currentLinePoints.ToArray());
		}

        void populateTiles()
		{
			foreach (Transform tile in transform)
			{
				tile.gameObject.AddComponent<SplineTile>();
				tiles.Add(tile);

                var splineTile = tile.GetComponent<SplineTile>();
                splineTile.setSpline(this);

				if (tile.childCount > 0)
				{
					var splineBox = tile.Find("SplineBox").GetComponent<SplineBox>();
					splineTile.setSplineBox(splineBox);
				}
			}
		}

        void depopulateTiles()
		{
			foreach (Transform tile in transform)
			{
				tile.GetComponent<SplineTile>().clearAllAttributes();
			}

			tiles.Clear();
		}

		/// <summary>
		/// Handles the input. FOR DEBUGGING ONLY. remove afterwards.
		/// </summary>
		void handleInput()
		{
			if (Input.GetKeyDown (KeyCode.Comma))
			{
				Debug.Log ("Comma Pressed");
				activateTileField ();
			}

			if (Input.GetKeyDown (KeyCode.Period))
			{
				Debug.Log ("Slash Pressed");
				deactivateTileField ();
			}
		}

		// Remove after debugging
		void Update () {
			handleInput ();
		}
	}
}
