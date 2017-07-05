using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBoxScoring : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void checkScoring()
	{
		foreach (Transform child in transform) 
		{
			if (!child.GetComponent<BodyBoxSwitch> ().isPressedFully)
				return;
		}
		puzzleCompleted ();
	}

	public void puzzleCompleted ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
