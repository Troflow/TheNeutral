using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRing : MonoBehaviour {

	// TODO: change to static constant
	[SerializeField]
	private float colourTransferTime;
	[SerializeField]
	private string ringColour;

	private void startGrantingColour(PlayerState pState)
	{
		StartCoroutine(grantColour(pState));
	}

	private IEnumerator grantColour(PlayerState pState)
	{
		//Debug.Log ("In Coroutine");
		yield return new WaitForSeconds (colourTransferTime);
		pState.heldColour = ringColour;
		//Debug.Log ("After Coroutine");
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			//Debug.Log ("Player Collided");
			startGrantingColour (col.GetComponent<PlayerState>());
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			//Debug.Log ("Exit Coroutine");
			StopCoroutine (grantColour (col.GetComponent<PlayerState>()));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
