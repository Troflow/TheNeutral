using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWall : MonoBehaviour {

	private string desiredColour;
	public bool isColouredWell;

	// Use this for initialization
	void Start () {
		
	}

	private void checkPlayerColour(string pPlayerColour)
	{
		if (pPlayerColour == desiredColour) 
		{
			//isColouredWell = true;
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			checkPlayerColour(col.GetComponent<PlayerState>().heldColour);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
