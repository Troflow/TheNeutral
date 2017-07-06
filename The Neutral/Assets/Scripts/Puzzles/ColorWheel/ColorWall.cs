using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ColorWall : MonoBehaviour {

	private ColorWheel colourWheel;
	[SerializeField]
	private Lite desiredColour;
	public bool isColouredCorrect;

	// Use this for initialization
	void Start () {
		colourWheel = transform.parent.GetComponent<ColorWheel> ();
	}

	private void checkPlayerColour(Lite pPlayerColour)
	{
		if (pPlayerColour == desiredColour) 
		{
			isColouredCorrect = true;
			colourWheel.halt ();
		}
	}

	#region Collision Handling
	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			checkPlayerColour(col.GetComponent<PlayerState>().heldColour);
		}
	}
	#endregion
}
