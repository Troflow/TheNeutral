
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Neutral
{
	/// <summary>
	/// Class handling the Spline puzzle. Splines are based on Numberlinks:
	/// https://en.wikipedia.org/wiki/Numberlink
	/// There is a given number of SplineLines to connect from origin to destination,
	/// and once all lines are connected the puzzle is completed.
	/// SplineLines of the same type cannot collide with each other. No SplineLine
	/// can collide with a Persistent SplineLineType.
	/// </summary>
	public class Spline : MonoBehaviour {

		bool isCompleted;

		Transform tileField;

        float splineLineOffset = 5;
		bool isActivated = false;

		// The SplineLine currently being drawn by the player
		SplineLine currentSplineLine;

		// Those SplineTiles which the currentSplineLine has been drawn over
		IDictionary<string, Vector3> touchedTiles;

		// The number of SplineLines to connect to complete the puzzle
		int splineLineCount;

		// The number of SplineLines currently connected by the player
		int completedSplineLineCount = 0;

		// For Debugging
		// TODO: Find a way to dynamically set the linePrefab for the SplineTiles
		// Current means is to pass this linePrefab to the activate() call of the SplineTile
		[SerializeField]
		Transform linePrefab;

		void activateSpline()
		{
			if (isActivated) return;

			tileField = transform.Find("TileField");
			touchedTiles = new Dictionary<string, Vector3>();

			activateTilesAndBoxes();

			isActivated = true;

			// For Debugging. Find a way to dynamically set the linePrefab, rather than
			// doing it this way
			foreach (Transform tile in tileField)
			{
				tile.GetComponent<SplineTile>().activate(linePrefab);
			}
		}

		void deactivateSpline()
		{
			if (!isActivated) return;

			deactivateTilesAndBoxes();

			touchedTiles = null;
			currentSplineLine = null;
			tileField = null;
			isActivated = false;

			if (!isCompleted) completedSplineLineCount = 0;
		}

		public bool getIsActivated()
		{
			return isActivated;
		}

		/// <summary>
		/// Called by a SplineTile that has a Standard SplineBox to update which
		/// line is currently being drawn by the player.
		/// </summary>
		/// <param name="pLine"></param>
		public void setCurrentSplineLine(SplineLine pLine)
		{
			// Clear the previous SplineLine of all its points if it isn't null
			// i.e. it hasn't been completed
			if (currentSplineLine != null)
			{
				currentSplineLine.removeAllPoints();
				touchedTiles.Clear();
			}

			currentSplineLine = pLine;
			currentSplineLine.clearAllTileOccupations();
		}

		public SplineLine getCurrentSplineLine()
		{
			return currentSplineLine;
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
			// TODO: Handle Puzzle completion properly
			if (completedSplineLineCount == splineLineCount)
			{
				isCompleted = true;
				// TODO: Uncomment after testing is finished
				// throw new System.NotImplementedException("Spline Puzzle Completion Logic Needed");
			}
		}

		public void addTileToCurrentLine(string pSplineTileName, Vector3 pTilePos)
		{
			// Use a dict to store touched tiles, because comparing on float values
			// of the Vector3 positions is non-deterministic
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

		/// <summary>
		/// Connects Standard SplineBoxes with their siblings. There must be an even
		/// number (or 0) of SplineBoxes or else an exception is thrown
		/// All SplineBoxes must follow the given format: SplineBox_[Color]_[Index]
		/// to be sorted correctly.
		/// </summary>
		/// <param name="pAllSplineBoxes"></param>
		void pairAllSplineBoxes(List<SplineBox> pAllSplineBoxes)
		{
			var splineBoxCount = pAllSplineBoxes.Count;

			// If not 0, or an even number of SplineBoxes in pAllSplineBoxes
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

        void activateTilesAndBoxes()
		{
			var allSplineBoxes = new List<SplineBox>();

			foreach (Transform tile in tileField)
			{
				tile.gameObject.AddComponent<SplineTile>();

                var splineTile = tile.GetComponent<SplineTile>();
                splineTile.setSpline(this);

				if (tile.childCount > 0)
				{
					// Add the splineBox to allSplineBoxes. If no Component is found, don't add
					var splineBox = tile.GetChild(0).GetComponent<SplineBox>();
					splineTile.setSplineBox(splineBox);
					if (splineBox != null && splineBox.getType() != SplineBoxType.Blockade)
					{
						allSplineBoxes.Add(splineBox);
					}
				}
			}

			pairAllSplineBoxes(allSplineBoxes);
		}

        void deactivateTilesAndBoxes()
		{
			if (tileField == null) return;

			foreach (Transform tile in tileField)
			{
				tile.GetComponent<SplineTile>().deactivate();
				Destroy(tile.GetComponent<SplineTile>());
				print(tile.name);
			}
		}

		/// <summary>
		/// For Debugging
		/// </summary>
		void handleInput()
		{
			if (Input.GetKeyDown (KeyCode.Comma))
			{
				Debug.Log ("Comma Pressed");
				activateSpline ();
			}

			if (Input.GetKeyDown (KeyCode.Period))
			{
				Debug.Log ("Slash Pressed");
				deactivateSpline ();
			}
		}

		// For Debugging
		void Update () {
			handleInput ();
		}
	}
}
