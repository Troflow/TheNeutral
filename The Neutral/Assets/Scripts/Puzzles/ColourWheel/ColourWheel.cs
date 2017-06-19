using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWheel : MonoBehaviour {

	public int haltOrder;
	[SerializeField]
	private float spinSpeed;
	[SerializeField]
	private bool isClockwise = true;
	public bool isHalted = false;
	private Vector3 spinVector;
	[SerializeField]
	private ColourWall colourWall;

	// Use this for initialization
	void Start () {
		if (!isClockwise) 
		{
			spinSpeed *= -1f;
		}
		spinVector = new Vector3 (0, spinSpeed, 0);
		colourWall = transform.GetChild(1).GetComponent<ColourWall> ();
	}

	public void spin()
	{
		if (!colourWall.isColouredWell) 
		{
			transform.Rotate (spinVector * Time.deltaTime);
			isHalted = true;
		}
	}
		
	
	// Update is called once per frame
	void Update () {
		spin ();
	}
}
