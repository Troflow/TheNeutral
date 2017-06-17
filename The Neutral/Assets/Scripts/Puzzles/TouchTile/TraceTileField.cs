using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceTileField : TouchTileField {

	[SerializeField]
	private Transform tracedLineObject;
	private LineRenderer tracedLine;
	[SerializeField]
	private float tracedLineOffset;
	[SerializeField]
	private List<Vector3> touchedTiles;

	[SerializeField]
	private bool firstPointPlaced;

	// Use this for initialization
	void Start () {
		defaultTileAwareState = true;
		tiles = new List<Transform>();
		populateTiles ();
		tileCount = tiles.Count;

		touchedTiles = new List<Vector3> ();
		tracedLine = tracedLineObject.GetComponent<LineRenderer> ();
		firstPointPlaced = false;
	}

	/// <summary>
	/// Handles the input. FOR DEBUGGING ONLY
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

	public override void touched (Transform pTile)
	{
		if (!isActivated)
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
		Vector3 tempPos = pTile.position;
		tempPos.y += tracedLineOffset;
		tracedLineObject.position = tempPos;

		firstPointPlaced = true;
	}

	private void addTracedTile(Transform pTile)
	{
		Vector3 tempPos;
		if (!touchedTiles.Contains (pTile.position)) 
		{
			tempPos = pTile.position;
			tempPos.y += tracedLineOffset;
			touchedTiles.Add (tempPos);
		}
	}

	private void removeTracedTile()
	{
		Vector3 tempPos;
		if (touchedTiles.Count > 1) 
		{
			touchedTiles.RemoveAt (touchedTiles.Count - 1);
			updateTracedLinePositions ();
		}
	}

	private void updateTracedLinePositions()
	{
		tracedLine.positionCount = touchedTiles.Count;
		Vector3[] positionsArray = touchedTiles.ToArray ();
		tracedLine.SetPositions (positionsArray);
	}

	protected override void showPrize ()
	{
		throw new System.NotImplementedException ();
	}

	// Update is called once per frame
	void Update () {
		handleInput ();
	}
}
