using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
	// All HUDElements must be added to the Script Execution Order to prevent
	// NullReferenceExeption Errors
	public class HeldColor : HUDElement, IObserver<PlayerState>
	{
		private Transform R;
		private Transform G;
		private Transform B;

		void OnEnable ()
		{
			getChildren();
			deactivateAllChildren();
			HUDManager.Subscribe (this);
		}

		void OnDisable ()
		{
			HUDManager.Dispose (this);
		}

		void updateHeldColorImage(Color heldColor)
		{
			var threshold = GameManager.colorSymbolDisplayThreshold;

			R.gameObject.SetActive(heldColor.r > threshold);
			G.gameObject.SetActive(heldColor.g > threshold);
			B.gameObject.SetActive(heldColor.b > threshold);
		}

		void getChildren()
		{
			R = transform.Find("R");
			G = transform.Find("G");
			B = transform.Find("B");
		}

		void deactivateAllChildren()
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Update this instance of HUD observer
		/// </summary>
		/// <param name="newState">New state.</param>
		public void OnNext(PlayerState newState)
		{
			var currentCombatColor = newState.getCurrentCombatColor();
			updateHeldColorImage(currentCombatColor.color.Value);
		}
	}
}