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

	public abstract void touched(Transform pTile);
	public abstract void interactedWith();
	protected abstract void showPrize ();

	protected virtual void populateTiles ()
	{
		int inc = 0;
		foreach (Transform child in transform) 
		{
			child.gameObject.AddComponent<TouchTile>();
			tiles.Add(child);
		}
	}

}
