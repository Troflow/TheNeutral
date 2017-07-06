using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceTileField : TouchTileField {

	[SerializeField]
	private Transform tracedLineObject;

	private LineRenderer tracedLine;
	private float tracedLineOffset = 3.5f;
	private List<Vector3> touchedTiles;
	private bool firstPointPlaced;

	void Start () {
		// Set default state to true so all tiles are
		// listening for contact, to allow drawing
		defaultTileAwareState = true;

		// Instantiate Tiles attributes
		tiles = new List<Transform>();
		populateTiles ();
		touchedTiles = new List<Vector3> ();
		tracedLine = tracedLineObject.GetComponent<LineRenderer> ();
		firstPointPlaced = false;
	}

	/// <summary>
	/// Handles the input. FOR DEBUGGING ONLY. remove afterwards.
	/// </summary>
	private void handleInput()
	{
		if (Input.GetKeyDown (KeyCode.Comma)) 
		{
			Debug.Log ("Comma Pressed");
			activateTileField ();
		}

		if (Input.GetKeyDown (KeyCode.Slash)) 
		{
			Debug.Log ("Slash Pressed");
			deactivateTileField ();
		}

		if (Input.GetKeyDown (KeyCode.RightShift)) 
		{
			Debug.Log ("Right Shift Pressed");
			removeTracedTile ();
		}
	}

	protected override void activateTileField ()
	{
		tracedLineObject.gameObject.SetActive (true);
		isActivated = true;
	}
		
	private void deactivateTileField()
	{
		isActivated = false;
		firstPointPlaced = false;

		tracedLine.positionCount = 0;
		touchedTiles.Clear ();

		tracedLineObject.gameObject.SetActive (false);
	}

	public override void touched (bool tileIsAware, Transform pTile, Transform pPlayerTransform)
	{
		// TODO: change so, if not activated and the player
		// is using a Tier when making contact with the tile
		// then the field should be activated
		if (!isActivated) // && pPlayerTransform.isUsingTier
			//activateTileField ();
			return;

		if (!firstPointPlaced) 
		{
			placeFirstPoint (pTile);
		}
		else 
		{
			addTracedTile (pTile);
			updateTracedLinePositions ();
		}
			
	}

	private void placeFirstPoint(Transform pTile)
	{
		addTracedTile (pTile);

		var tempPos = pTile.position;
		tempPos.y += tracedLineOffset;
		tracedLineObject.position = tempPos;

		firstPointPlaced = true;
	}

	private void addTracedTile(Transform pTile)
	{
		var tempPos = new Vector3 ();;
		if (!touchedTiles.Contains (pTile.position)) 
		{
			tempPos = pTile.position;
			tempPos.y += tracedLineOffset;
			touchedTiles.Add (tempPos);
		}
	}

	private void removeTracedTile()
	{
		var tempPos = new Vector3();
		if (touchedTiles.Count > 1) 
		{
			touchedTiles.RemoveAt (touchedTiles.Count - 1);
			updateTracedLinePositions ();
		}
	}

	private void updateTracedLinePositions()
	{
		tracedLine.positionCount = touchedTiles.Count;

		var positionsArray = touchedTiles.ToArray ();
		tracedLine.SetPositions (positionsArray);
	}

	protected override void puzzleCompleted ()
	{
		throw new System.NotImplementedException ();
	}

	// Remove after debugging
	void Update () {
		handleInput ();
	}
}
