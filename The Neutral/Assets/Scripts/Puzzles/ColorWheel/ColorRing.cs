using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ColorRing : MonoBehaviour {

	private float colorTransferTime;
	[SerializeField]
	private Lite ringColour;

	public void setTransferTime(float pTransferTime)
	{
		colorTransferTime = pTransferTime;
	}

	private void startGrantingColour(PlayerState pState)
	{
		StartCoroutine(grantColour(pState));
	}

	//TODO: Hussain: timing isn't working correctly. Not always waiting the
	// given amount
	private IEnumerator grantColour(PlayerState pState)
	{
		yield return new WaitForSeconds (colorTransferTime);
		pState.heldColour = ringColour;
	}

	#region Collision Handling
	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			startGrantingColour (col.GetComponent<PlayerState>());
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			StopCoroutine (grantColour (col.GetComponent<PlayerState>()));
		}
	}
	#endregion
}
