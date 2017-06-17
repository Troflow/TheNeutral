using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchTileField : MonoBehaviour {

	/// <summary>
	/// Array of tiles for this TouchTile instance
	/// </summary>
	[SerializeField]
	protected List<Transform> tiles;
	[SerializeField]
	protected int tileCount;
	[SerializeField]
	protected bool isActivated;
	protected bool defaultTileAwareState;

	public abstract void touched(Transform pTile);
	protected abstract void activateTileField ();
	protected abstract void showPrize ();

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
