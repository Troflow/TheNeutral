﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWall : MonoBehaviour {

	private ColourWheel colourWheel;
	[SerializeField]
	private string desiredColour;
	public bool isColouredCorrect = false;

	// Use this for initialization
	void Start () {
		colourWheel = transform.parent.GetComponent<ColourWheel> ();
	}

	private void checkPlayerColour(string pPlayerColour)
	{
		if (pPlayerColour == desiredColour) 
		{
			Debug.Log ("Colours Match: " + pPlayerColour + " " + desiredColour);
			isColouredCorrect = true;
			colourWheel.halt ();
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
