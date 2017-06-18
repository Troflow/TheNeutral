using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWheel : MonoBehaviour {

	private string wheelColour;
	[SerializeField]
	private float spinSpeed;
	[SerializeField]
	private bool isClockwise = true;
	private Vector3 spinVector;
	// Use this for initialization
	void Start () {
		if (!isClockwise) 
		{
			spinSpeed *= -1f;
		}
		spinVector = new Vector3 (0, spinSpeed, 0);
	}

	private void spin()
	{
		transform.Rotate (spinVector * Time.deltaTime);
	}

	
	// Update is called once per frame
	void Update () {
		spin ();
	}
}
