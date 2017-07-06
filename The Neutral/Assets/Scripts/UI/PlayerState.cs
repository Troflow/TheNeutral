using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerState : MonoBehaviour 
{

	[SerializeField]
	private HUDManager _HUD;

	public int stamina;
	public Lite heldColour;
	public Dictionary<Lite, int> completedPuzzles;

	void Awake()
	{
		stamina = 100;
		populateCompletedPuzzles ();
	}

	private void populateCompletedPuzzles()
	{
		completedPuzzles = new Dictionary<Lite, int> ();

		completedPuzzles.Add (Lite.GREEN, 0);
		completedPuzzles.Add (Lite.BLUE, 0);
		completedPuzzles.Add (Lite.YELLOW, 0);
		completedPuzzles.Add (Lite.RED, 0);
		completedPuzzles.Add (Lite.GRAY, 0);
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
