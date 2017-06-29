using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour {

	//private List<ColourWheel> colourWheels;
	[SerializeField]
	private ColourWheel centre;
	[SerializeField]
	private List<ColourWheel> haltedColourWheels;
	private bool correctlyOrdered;


	// Use this for initialization
	void Start () {
		haltedColourWheels = new List<ColourWheel> ();
	}

	public void addWheel(ColourWheel pColourWheel)
	{
		if (!haltedColourWheels.Contains (pColourWheel)) 
		{
			haltedColourWheels.Add (pColourWheel);
			validateHaltedWheels ();
		}
	}

	private void validateHaltedWheels()
	{
		Debug.Log ("Validating Halted Wheels");
		if (haltedColourWheels.Count < 2) return;

		// Loop through haltedColourWheels, making sure each wheel's halt order
		// matches their index in the list
		foreach (ColourWheel wheel in haltedColourWheels) 
		{
			if (wheel.haltOrder != haltedColourWheels.IndexOf (wheel)) 
			{
				correctlyOrdered = false;
				return;	
			}
		}

		correctlyOrdered = true;
		endPuzzle ();
	}

	private void endPuzzle()
	{
	}

	// Update is called once per frame
	void Update () {
	}
}
