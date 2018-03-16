using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public abstract class TouchTileField : MonoBehaviour {

		/*
		/// <summary>
		/// Array of tiles for this TouchTile instance
		/// </summary>
		protected List<Transform> tiles;
		/// <summary>
		/// The default state of the tile at start.
		/// </summary>
		protected bool defaultTileAwareState;

        public abstract void touched(bool tileIsAware, Transform pTile, Transform pPlayerTransform);

		protected abstract void activateTileField ();

		protected virtual void populateTiles ()
		{
			foreach (Transform tile in transform)
			{
				tile.gameObject.AddComponent<SplineTile>();
				tile.gameObject.GetComponent<SplineTile> ().isAware = defaultTileAwareState;
				tiles.Add(tile);

				if (tile.childCount > 0)
				{
					//TODO: Remove all references to SplineTile.
					var splineTile = tile.GetComponent<SplineTile>();
					var splineBox = tile.Find("SplineBox").GetComponent<SplineBox>();
					splineTile.setSplineBox(splineBox);
				}
			}
		}

		protected virtual void depopulateTiles()
		{
			foreach (Transform tile in transform)
			{
				tile.GetComponent<SplineTile>().clearAllAttributes();
			}

			tiles.Clear();
		}

	*/}
}
