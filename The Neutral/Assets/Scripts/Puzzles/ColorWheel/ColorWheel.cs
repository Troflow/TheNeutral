using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour {

	private Carousel carousel;
	public int haltOrder;
	[SerializeField]
	private float spinSpeed;
	[SerializeField]
	private bool isClockwise = true;
	public bool isHalted = false;
	private Vector3 spinVector;

	// Use this for initialization
	void Start () {
		carousel = transform.parent.GetComponent<Carousel> ();

		if (!isClockwise) 
		{
			spinSpeed *= -1f;
		}
		spinVector = new Vector3 (0, spinSpeed, 0);
	}

	public void halt()
	{
		isHalted = true;
		carousel.addWheel (this);
	}

	public void spin()
	{
		if (!isHalted) 
		{
			transform.Rotate (spinVector * Time.deltaTime);
		}
	}
		
	
	// Update is called once per frame
	void Update () {
		spin ();
	}
}
