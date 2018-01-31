using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
	// All HUDElements must be added to the Script Execution Order to prevent
	// NullReferenceExeption Errors
	public class StaminaText : HUDElement, IObserver<PlayerState>
	{
		/// <summary>
		/// The text component of this instance
		/// </summary>
		private Text staminaText;

		void OnEnable()
		{
			staminaText = GetComponent<Text>();

			// When set to active,
			// add this instance of IHUDObserver to the HUDManager's list of observers
			HUDManager.Subscribe (this);
		}

		void OnDisable()
		{
			// When set to active,
			// add this instance of IHUDObserver to the HUDManager's list of observers
			HUDManager.Dispose (this);
		}

		/// <summary>
		/// Update this instance of HUD observer
		/// change text value to reflect current amount of stamina
		/// </summary>
		/// <param name="updatedData">Updated info.</param>
		public void OnNext(PlayerState newState)
		{
			staminaText.text = "" + newState.stamina;
		}
	}
}
