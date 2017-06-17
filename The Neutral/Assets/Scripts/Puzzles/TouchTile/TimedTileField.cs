using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTileField : TouchTileField {

	[SerializeField]
	private Transform tileMarker;
	[SerializeField]
	private Transform markedTile;
	[SerializeField]
	private int tileScore;
	[SerializeField]
	private int targetTileScore;
	[SerializeField]
	private float timerDuration;
	[SerializeField]
	private float markerOffset;

	// Use this for initialization
	void Start () {
		defaultTileAwareState = false;
		tiles = new List<Transform>();
		populateTiles ();
		tileCount = tiles.Count;

		// For debugging purposes. Actual activation will be handled
		// within interactedWith() 
		activateTileField ();
	}
		
	/// <summary>
	/// Activates the tile field.
	/// Sets isActivated to true, picks a random tile to be 'marked'
	/// moves the tileMarker to the position of the marked tile
	/// </summary>
	protected override void activateTileField()
	{
		isActivated = true;
		markedTile = tiles [Random.Range (0, tiles.Count)];
		markedTile.GetComponent<TouchTile> ().isAware = true;
		//StartCoroutine (startTimer);
		placeInitialMarker ();
	}

	/// <summary>
	/// Deactivates the tile field.
	/// Stops the timer. Sets the field's isActivated to false
	/// and hides the tileMarker
	/// </summary>
	private void deactivateTileField()
	{
		isActivated = false;
		StopCoroutine (startTimer ());
		tileMarker.gameObject.SetActive (false);
	}
		
	/// <summary>
	/// Called during activateTileField. 
	/// Sets the tileMarker to active
	/// then places the  tileMarker in its initial position - above the markedTile
	/// </summary>
	private void placeInitialMarker()
	{
		tileMarker.gameObject.SetActive (true);
		Vector3 markerPosition = markedTile.position;
		markerPosition.y += markerOffset;
		tileMarker.position = markerPosition;
	}


	/// <summary>
	/// Displaces the tileMarker. By changing the old markedTile
	/// Picking a new markedTile randomly from the list of tiles
	/// Setting this new tile's isMarkedTile to true
	/// and finally, moving the tileMarker to the location of the new markedTile
	/// </summary>
	private void displaceMarker()
	{
		// set the current markedTile as no longer marked
		markedTile.GetComponent<TouchTile>().isAware = false;
			
		// Duplicate tiles List, and remove the markedTile from duplicateList
		List<Transform> duplicateList = new List<Transform>(tiles);
		duplicateList.Remove (markedTile);

		// Pick a tile from from duplicateList. At random. Set it as isMarkedTile
		markedTile = duplicateList[Random.Range(0, duplicateList.Count)];
		markedTile.GetComponent<TouchTile> ().isAware = true;

		// Place the tileMarker at the position of the markedTile. Offset upwards
		Vector3 markerPosition = markedTile.position;
		markerPosition.y += markerOffset;
		tileMarker.position = markerPosition;
	}


	public override void touched(Transform pTile)
	{ 
		if(isActivated) 
		{
			incrementTileScore ();
			displaceMarker ();
			//resetTimer ();
		}
	}
		
	private void incrementTileScore()
	{
		tileScore++;

		if (tileScore >= targetTileScore) 
		{
			deactivateTileField ();
			showPrize ();
		}
	}

	protected override void showPrize ()
	{
		//throw new System.NotImplementedException ();
	}

	/// <summary>
	/// Updates the timer.
	/// By stopping the ongoing timer coroutine. And resetting it.
	/// </summary>
	private void resetTimer()
	{
		StopCoroutine (startTimer ());
		StartCoroutine (startTimer ());
	}

	private IEnumerator startTimer()
	{
		yield return new WaitForSeconds(timerDuration);
		deactivateTileField ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
