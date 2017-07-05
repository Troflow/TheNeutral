using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour 
{

	[SerializeField]
	private HUDManager _HUD;

	public int stamina;
	public string heldColour;
	public Dictionary<string, int> completedPuzzles;

	void Awake()
	{
		stamina = 100;
		populateCompletedPuzzles ();
	}

	private void populateCompletedPuzzles()
	{
		completedPuzzles.Add ("Blue_Puzzle", 0);
		completedPuzzles.Add ("Red_Puzzle", 0);
		completedPuzzles.Add ("Green_Puzzle", 0);
		completedPuzzles.Add ("Yellow_Puzzle", 0);
		completedPuzzles.Add ("Purple_Puzzle", 0);
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
