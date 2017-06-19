using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour 
{

	[SerializeField]
	private HUDManager _HUD;

	public int stamina;
	public string heldColour;

	void Awake()
	{
		stamina = 100;
	}

	public void onStateChange()
	{
		_HUD.NotifyAllObservers ();
	}

	private void handleInput()
	{
		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			stamina -= 1;
			onStateChange ();
		}

		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			stamina += 1;
			onStateChange ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		handleInput ();
	}
}
