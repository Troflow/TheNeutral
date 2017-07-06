using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BodyBoxSwitch : MonoBehaviour {

	public Lite desiredColor;

	private Vector3 switchPressedPosition;
	private Vector3 switchRaisedPosition;
	private float smoothing = 2f;

	private bool isDisplacedDown;
	private bool isDisplacedUp;
	public bool isCoveredCorrectly;

	void Start () {
		setPositions ();
	}

	/// <summary>
	/// Sets where the switch will be at its highest and lowest
	/// points
	/// </summary>
	private void setPositions()
	{
		switchPressedPosition = transform.position;
		switchPressedPosition.y -= 2f;
		switchRaisedPosition = transform.position;
		switchRaisedPosition.y += 2f;
	}

	/// <summary>
	/// Called while switch is covered by a BodyBox
	/// </summary>
	private void depress()
	{
		transform.position = Vector3.Lerp (transform.position, switchPressedPosition, smoothing * Time.deltaTime);
		if (Vector3.Distance (transform.position, switchPressedPosition) < 0.5f) 
		{
			isDisplacedDown = true;
			isDisplacedUp = false;
		}
	}

	/// <summary>
	/// Called while switch is not at its highest point, and 
	/// is not currently covered by a BodyBox
	/// </summary>
	private void rise()
	{
		transform.position = Vector3.Lerp (transform.position, switchRaisedPosition, smoothing * Time.deltaTime);
		if (Vector3.Distance (transform.position, switchRaisedPosition) < 0.5f) 
		{
			isDisplacedDown = false;
			isDisplacedUp = true;
		}
	}

	/// <summary>
	/// Called when a BodyBox comes into contact
	/// with this BodyBoxSwitch instance. Checks that switch and box
	/// have the same desired Lite coloring
	/// </summary>
	/// <returns><c>true</c>, if check was colored, <c>false</c> otherwise.</returns>
	/// <param name="pBodyBox">P body box.</param>
	private bool colorCheck(BodyBox pBodyBox)
	{
		return pBodyBox.boxColor == desiredColor;
	}

	#region Collision Handling
	public void OnTriggerStay(Collider col)
	{
		if (col.CompareTag ("BodyBox") && colorCheck(col.GetComponent<BodyBox>())) {
			isCoveredCorrectly = true;
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.CompareTag ("BodyBox") && colorCheck(col.GetComponent<BodyBox>())) 
		{
			isCoveredCorrectly = false;
		}
	}
	#endregion
		
	void Update () {
		
		if (isCoveredCorrectly && !isDisplacedDown) 
		{
			depress ();
		}

		if (!isCoveredCorrectly && !isDisplacedUp) 
		{
			rise ();
		}
	}
}
