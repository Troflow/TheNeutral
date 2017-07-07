using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class TimedTileField : TouchTileField {

		[SerializeField]
		private Transform tileMarker;
		private Transform markedTile;
		private int tileCount;
		private int tileScore;
		private int targetTileScore = 5;
		private float timerDuration = 10f;
		private float markerOffset = 5f;

		void Start () {
			// Set default state to false so only the marked
			// tile will be chosen to listen for contact with player.
			defaultTileAwareState = false;

			tiles = new List<Transform>();
			populateTiles ();
			tileCount = tiles.Count;

			// For debugging purposes. 
			//Actual activation will be handled in touched() 
			// only when player uses a Tier on one of the TouchTiles.
			activateTileField ();
		}
			
		/// <summary>
		/// Activates the tile field.
		/// Sets isActivated to true, picks a random tile to be 'marked'
		/// moves the tileMarker to the position of the marked tile.
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
		/// and hides the tileMarker.
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
		/// then places the  tileMarker in its initial position - above the markedTile.
		/// </summary>
		private void placeInitialMarker()
		{
			tileMarker.gameObject.SetActive (true);

			var markerPosition = markedTile.position;
			markerPosition.y += markerOffset;
			tileMarker.position = markerPosition;
		}


		/// <summary>
		/// Displaces the tileMarker. By changing the old markedTile
		/// Picking a new markedTile randomly from the list of tiles
		/// Setting this new tile's isMarkedTile to true
		/// and finally, moving the tileMarker to the location of the new markedTile.
		/// </summary>
		private void displaceMarker()
		{
			// set the current markedTile as no longer marked
			markedTile.GetComponent<TouchTile>().isAware = false;
				
			// Duplicate tiles List, and remove the markedTile from duplicateList
			var duplicateList = new List<Transform>(tiles);
			duplicateList.Remove (markedTile);

			// Pick a tile from from duplicateList. At random. Set it as isMarkedTile
			markedTile = duplicateList[Random.Range(0, duplicateList.Count)];
			markedTile.GetComponent<TouchTile> ().isAware = true;

			// Place the tileMarker at the position of the markedTile. Offset upwards
			Vector3 markerPosition = markedTile.position;
			markerPosition.y += markerOffset;
			tileMarker.position = markerPosition;
		}


		public override void touched(bool tileIsAware, Transform pTile, Transform pPlayerTransform)
		{ 
			// TODO: Pseudo code. Field should activate
			// once player is in contact with any of the tiles
			// AND player uses a Tier move
			if (!isActivated) //&& pPlayerTransform.isUsingTier) 
			{
				//activateTileField ();
			}

			if(isActivated && tileIsAware) 
			{
				incrementTileScore ();
				displaceMarker ();
				//TODO: implement when ready:
				//resetTimer ();
			}
		}
			
		private void incrementTileScore()
		{
			tileScore++;

			if (tileScore >= targetTileScore) 
			{
				deactivateTileField ();
			}
		}

		protected override void puzzleCompleted ()
		{
			throw new System.NotImplementedException ();
		}

		// TODO: may need to be refactored
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
	}
}
