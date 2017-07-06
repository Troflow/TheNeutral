using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// Color wall class, responsible for tracking its current color,
/// and telling its parent color wheel to 'halt' when
/// this wall is colored properly
/// </summary>
public class ColorWall : MonoBehaviour {

	private ColorWheel colorWheel;
	[SerializeField]
	private Lite desiredColor;
	public bool isColoredCorrectly;

	void Start () {
		colorWheel = transform.parent.GetComponent<ColorWheel> ();
	}

	private void checkPlayerColor(Lite pPlayerColor)
	{
		if (pPlayerColor == desiredColor) 
		{
			isColoredCorrectly = true;
			colorWheel.halt ();
		}
	}

	#region Collision Handling
	public void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag ("Player")) 
		{
			checkPlayerColor(col.GetComponent<PlayerState>().heldColour);
		}
	}
	#endregion
}
