using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class ColorBook : HUDElement, IObserver<PlayerState>
	{
		void OnEnable ()
		{
			HUDManager.Subscribe (this);
		}

		void OnDisable ()
		{
			HUDManager.Dispose (this);
		}

		void updateColorBookImage(List<CombatColor> colorBook)
		{
			// the colorBook image is comprised of
			// three R, G and B images.
			// Each image will only be displayed if the corresponding
			// RGB value of the exposedColor is not 0
		}

		/// <summary>
		/// Update this instance of HUD observer
		/// </summary>
		/// <param name="newState">New state.</param>
		public void OnNext(PlayerState newState)
		{
			updateColorBookImage(newState.colorBook);
		}
	}
}