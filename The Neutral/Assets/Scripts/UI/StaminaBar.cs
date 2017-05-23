using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour {

	[SerializeField]
	private Image _StaminaRing;

	[SerializeField]
	private Transform _StaminaIndicatorObject;

	[SerializeField]
	private TextMesh _Text;

	// Use this for initialization
	void Start () {

	}

	private void handleInput()
	{
		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			_StaminaRing.fillAmount -= 0.01f;
			handleIndicatorPosition ();
		}

		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			_StaminaRing.fillAmount += 0.01f;
			handleIndicatorPosition ();
		}
			
	}

	// TODO: Make so it only updates when Stamina is lost, rather than every frame.
	/// <summary>
	/// Handles StaminaIndicator position based on amount of Stamina lost between checks
	/// </summary>
	private void handleIndicatorPosition()
	{
		// Get the change in StaminaRing fillAmount
		float rotateAmountDegrees = (360f - (_StaminaRing.fillAmount*360)) + 180f;
		Debug.Log ("Rotate Amount: " + rotateAmountDegrees);

		// Multiply difference by 3.6 for degrees conversion. Rotate by that amount
		Vector3 newRotate = new Vector3 (0, Mathf.Clamp(rotateAmountDegrees, 0f, 540f), 0);

		//Quaternion newRotation = Quaternion.Euler (0, rotateAmountDegrees+180f, 0);

		//_StaminaIndicatorObject.rotation = Quaternion.identity;
		_StaminaIndicatorObject.eulerAngles = (newRotate);
		_Text.text = "" + (int)(_StaminaRing.fillAmount * 100);

	}


	// Update is called once per frame
	void Update () {
		handleInput();
	}
}
