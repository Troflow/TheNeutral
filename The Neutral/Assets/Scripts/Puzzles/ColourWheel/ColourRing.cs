using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourRing : MonoBehaviour {

	[SerializeField]
	private int colourTransferTime;
	[SerializeField]
	private string ringColour;

	private void startGrantingColour(PlayerState pState)
	{
		StartCoroutine(grantColour(pState));
	}

	private IEnumerator grantColour(PlayerState pState)
	{
		for (int x = 0; x >= colourTransferTime; x++) 
		{
			yield return new WaitForSeconds (1f);
			Debug.Log ("Time Passed: " + x);
		}

		pState.heldColour = ringColour;
		Debug.Log ("Player Held Colour Now: " + ringColour);
	}

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
			//StopCoroutine (grantColour (col.GetComponent<PlayerState>()));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
