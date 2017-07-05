using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBoxSwitch : MonoBehaviour {

	public string desiredColor;

	private Vector3 switchPressedPosition;
	private Vector3 switchRaisedPosition;
	private float smoothing;

	private bool isRaisedFully;
	public bool isPressedFully;
	private bool isCovered;

	// Use this for initialization
	void Start () {
		desiredColor = "RED";
		smoothing = 2f;
		setPositions ();
	}

	private void setPositions()
	{
		switchPressedPosition = transform.position;
		switchPressedPosition.y -= 2f;
		switchRaisedPosition = transform.position;
		switchRaisedPosition.y += 2f;
	}

	private void depress()
	{
		transform.position = Vector3.Lerp (transform.position, switchPressedPosition, smoothing * Time.deltaTime);
		if (Vector3.Distance (transform.position, switchPressedPosition) < 0.5f) 
		{
			isPressedFully = true;
			isRaisedFully = false;
		}
	}

	private void rise()
	{
		transform.position = Vector3.Lerp (transform.position, switchRaisedPosition, smoothing * Time.deltaTime);
		if (Vector3.Distance (transform.position, switchRaisedPosition) < 0.5f) 
		{
			isPressedFully = false;
			isRaisedFully = true;
		}
	}

	private bool colorCheck(BodyBox pBodyBox)
	{
		return pBodyBox.boxColor == desiredColor;
	}

	public void OnTriggerStay(Collider col)
	{
		if (col.CompareTag ("BodyBox") && colorCheck(col.GetComponent<BodyBox>())) {
			isCovered = true;
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.CompareTag ("BodyBox")) 
		{
			isCovered = false;
		}
	}
		
	// Update is called once per frame
	void Update () {
		
		if (isCovered && !isPressedFully) 
		{
			depress ();
		}

		if (!isCovered && !isRaisedFully) 
		{
			rise ();
		}
	}
}
