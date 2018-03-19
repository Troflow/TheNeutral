
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Neutral
{
	public class Spline : MonoBehaviour {

		[SerializeField]
		bool isCompleted;

		Transform tileField;
		List<Transform> tiles;

        float splineLineOffset = 5;
		bool isActivated = false;
		[SerializeField]
		SplineLine currentSplineLine;
		IDictionary<string, Vector3> touchedTiles;

		[SerializeField]
		int splineLineCount;
		[SerializeField]
		int completedSplineLineCount = 0;

		// For Debugging
		public Transform linePrefab;

		void Start () {
			tileField = transform.Find("TileField");
			populateTiles ();
			touchedTiles = new Dictionary<string, Vector3>();
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
			throw new System.NotImplementedException("Spline Puzzle Deactivation Logic Needed");
			isActivated = false;

			if (!isCompleted) completedSplineLineCount = 0;
			currentSplineLine = null;

			depopulateTiles();
		}

		public bool getIsActivated()
		{
			return isActivated;
		}

		public void setCurrentSplineLine(SplineLine pLine)
		{
			// Clear the previous SplineLine of all its points if it wasn't completed
			if (currentSplineLine != null)
			{
				currentSplineLine.clearAllTileOccupations();
				currentSplineLine.removeAllPoints();

				touchedTiles.Clear();
			}

			currentSplineLine = pLine;
			currentSplineLine.clearAllTileOccupations();
		}

		public void clearCurrentSplineLine()
		{
			if (currentSplineLine != null)
			{
				currentSplineLine.clearAllTileOccupations();
				currentSplineLine.removeAllPoints();
				currentSplineLine = null;

				touchedTiles.Clear();
			}
		}

		public SplineLine getCurrentSplineLine()
		{
			return currentSplineLine;
		}

		public SplineLineType getCurrentSplineLineType()
		{
			return currentSplineLine.getType();
		}

		public bool checkIfIsDestination(SplineBox pSplineBox)
		{
			if (pSplineBox == currentSplineLine.getDestination()) return true;
			else return false;
		}

		public void reduceCompletedSplineCountBy(int pDecrementValue)
		{
			completedSplineLineCount = completedSplineLineCount - pDecrementValue;
		}

		public void splineLineCompleted(SplineBox pSplineBox)
		{
			pSplineBox.setSplineLineConnected(true);
			pSplineBox.getSibling().setSplineLineConnected(true);

			currentSplineLine = null;
			touchedTiles.Clear();

			completedSplineLineCount++;
			checkIsCompleted();
		}

		public void checkIsCompleted()
		{
			// TODO: Handle Puzzle completion
			if (completedSplineLineCount == splineLineCount)
			{
				isCompleted = true;
				// TODO: Uncomment after testing is finished
				// throw new System.NotImplementedException("Spline Puzzle Completion Logic Needed");
			}
		}

		public void addTileToCurrentLine(string pSplineTileName, Vector3 pTilePos)
		{
			var tilePos = new Vector3();
			if (!touchedTiles.ContainsKey(pSplineTileName))
			{
				tilePos = pTilePos;
				tilePos.y += splineLineOffset;

				touchedTiles.Add(pSplineTileName, tilePos);

				updateLineRendererPositions(currentSplineLine.getLineRenderer());
			}
		}

		public void updateLineRendererPositions(LineRenderer pLineRenderer)
		{
			pLineRenderer.positionCount = touchedTiles.Count;

			// TODO: Convert the list of SplineTile to list of Vector3
			pLineRenderer.SetPositions(touchedTiles.Values.ToArray());
		}

		void pairAllSplineBoxes(List<SplineBox> pAllSplineBoxes)
		{
			var splineBoxCount = pAllSplineBoxes.Count;

			// If not 0, or an even number of SplineBoxes
			if (splineBoxCount % 2 != 0)
			{
				throw new InvalidSplineBoxCountException("There should be an even number of SplineBoxes in this Spline.");
			}

			splineLineCount = splineBoxCount / 2;
			// SplineBoxes are sorted based on their naming scheme.
			// Will cause problems if SplineBoxes are not all correctly named.
			pAllSplineBoxes.Sort((x,y) => x.name.CompareTo(y.name));

			for (int i=0; i<splineBoxCount-1; i++)
			{
				var box0 = pAllSplineBoxes[i];
				var box1 = pAllSplineBoxes[++i];

				box0.setSibling(box1);
				box1.setSibling(box0);
			}

		}

        void populateTiles()
		{
			tiles = new List<Transform>();
			var allSplineBoxes = new List<SplineBox>();

			foreach (Transform tile in tileField)
			{
				tile.gameObject.AddComponent<SplineTile>();
				tiles.Add(tile);

                var splineTile = tile.GetComponent<SplineTile>();
                splineTile.setSpline(this);

				if (tile.childCount > 0)
				{
					// Add the SplineBox to allSplineBoxes. If no Component is found, don't add
					var splineBox = tile.GetChild(0).GetComponent<SplineBox>();
					splineTile.setSplineBox(splineBox);
					if (splineBox != null)
					{
						allSplineBoxes.Add(splineBox);
					}
				}
			}

			pairAllSplineBoxes(allSplineBoxes);
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
		/// FOR DEBUGGING ONLY.
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

		// For Debugging Only
		void Update () {
			handleInput ();
		}
	}
}
