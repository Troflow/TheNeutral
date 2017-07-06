using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchTileField : Puzzle {

	/// <summary>
	/// Array of tiles for this TouchTile instance
	/// </summary>
	protected List<Transform> tiles;
	protected bool isActivated;
	/// <summary>
	/// The default state of the tile at start.
	/// </summary>
	protected bool defaultTileAwareState;

	public abstract void touched(bool tileIsAware, Transform pTile, Transform pPlayerTransform);
	protected abstract void activateTileField ();

	protected virtual void populateTiles ()
	{
		foreach (Transform child in transform) 
		{
			child.gameObject.AddComponent<TouchTile>();
			child.gameObject.GetComponent<TouchTile> ().isAware = defaultTileAwareState;
			tiles.Add(child);
		}
	}

}
