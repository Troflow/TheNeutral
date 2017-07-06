using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// Color Ring class, responsible for granting the player
/// the designated ringColorgame after staying in contact with
/// the player for a given amount of time.
/// </summary>
public class ColorRing : MonoBehaviour {

	private float colorTransferTime;
	[SerializeField]
	private Lite ringColor;

	public void setTransferTime(float pTransferTime)
	{
		colorTransferTime = pTransferTime;
	}

	private void startGrantingColor(PlayerState pState)
	{
		StartCoroutine(grantColor(pState));
	}

	//TODO: Hussain: timing isn't working correctly. Method isn't always waiting the
	// given amount
	private IEnumerator grantColor(PlayerState pState)
	{
		yield return new WaitForSeconds (colorTransferTime);
		pState.heldColour = ringColor;
	}

	#region Collision Handling
	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			startGrantingColor (col.GetComponent<PlayerState>());
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			StopCoroutine (grantColor (col.GetComponent<PlayerState>()));
		}
	}
	#endregion
}
