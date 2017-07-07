using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class PlayerState : MonoBehaviour 
	{

		[SerializeField]
		private HUDManager HUD;

		#region Player Attributes
		public int stamina = 100;
		public Lite heldColor;

		public List<Lite> appliedStacks;

		public Dictionary<Lite, int> completedPuzzles;
		#endregion

		void Awake()
		{
			stamina = 100;
			populateCompletedPuzzles ();

			HUD.setPlayerState (this);
			HUD.setPlayerTransform (this.transform);
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
			HUD.NotifyAllObservers ();
		}

		// For Debugging purposes.
		private void handleInput()
		{
			if (Input.GetKey (KeyCode.DownArrow)) 
			{
				stamina -= 1;
				stamina = Mathf.Clamp (stamina, 0, 100);
				onStateChange ();
			}

			if (Input.GetKey (KeyCode.UpArrow)) 
			{
				stamina += 1;
				stamina = Mathf.Clamp (stamina, 0, 100);
				onStateChange ();
			}

		}
		
		// Update is called once per frame
		void Update () {
			handleInput ();
		}
	}
}
