using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
	public class StaminaRing : HUDElement, IObserver<PlayerState> 
	{
		private Image staminaRing;

		void OnEnable ()
		{
			staminaRing = GetComponent<Image> ();
			HUDManager.Subscribe (this);
		}
			
		void OnDisable () 
		{
			HUDManager.Dispose (this);
		}
			
		/// <summary>
		/// Update this instance of HUD observer
		/// </summary>
		/// <param name="newState">New state.</param>
		public void OnNext(PlayerState newState)
		{
			staminaRing.fillAmount = 0.01f * newState.stamina;
		}
	}
}
