using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carousel : Puzzle {

	/// <summary>
	/// The centre circle. 
	/// TODO: discuss with Hussain how this will be important
	/// during gameplay
	/// </summary>
	private Transform centre;
	private List<ColorWheel> haltedColourWheels;

	[SerializeField]
	private float colorTransferTime;
	private bool correctlyOrdered;


	// Use this for initialization
	void Start () {
		haltedColourWheels = new List<ColorWheel> ();
		setColorTransferTime ();

	}

	private void setColorTransferTime()
	{
		foreach (Transform child in transform) 
		{
			ColorRing ring = child.GetComponentInChildren<ColorRing> ();
			if(ring != null)
				ring.setTransferTime (colorTransferTime);
		}
	}

	public void addWheel(ColorWheel pColourWheel)
	{
		if (!haltedColourWheels.Contains (pColourWheel)) 
		{
			haltedColourWheels.Add (pColourWheel);
			validateHaltedWheels ();
		}
	}

	/// <summary>
	/// Once the number of halted wheels exceeds 2,
	/// will check to see the walls's index in the list
	/// matches with its haltOrder
	/// </summary>
	private void validateHaltedWheels()
	{
		if (haltedColourWheels.Count < 2) return;

		// Loop through haltedColourWheels, making sure each wheel's halt order
		// matches their index in the list
		foreach (ColorWheel wheel in haltedColourWheels) 
		{
			if (wheel.haltOrder != haltedColourWheels.IndexOf (wheel)) 
			{
				correctlyOrdered = false;
				return;	
			}
		}

		correctlyOrdered = true;
		puzzleCompleted ();
	}

	protected override void puzzleCompleted ()
	{
		throw new System.NotImplementedException ();
	}
}
