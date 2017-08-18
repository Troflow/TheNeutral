using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class StaminaText : HUDElement, IObserver<PlayerState> 
	{
		/// <summary>
		/// The text mesh component of this instance
		/// </summary>
		private TextMesh _TextMesh;

		void OnEnable()
		{
			_TextMesh = GetComponent<TextMesh>();

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
			_TextMesh.text = "" + newState.stamina;
		}
	}
}
