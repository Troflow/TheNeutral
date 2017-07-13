using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class ColorStack : HUDElement, IObserver<PlayerState> 
	{
	
		List<Transform> stacks;

		void OnEnable ()
		{
			HUDManager.Subscribe (this);
			setStacks ();
		}

		void OnDisable () 
		{
			HUDManager.Dispose (this);
		}

		private void setStacks()
		{
			stacks = new List<Transform> ();
			foreach (Transform child in transform) 
			{
				stacks.Add (child);
				child.GetComponent<SpriteRenderer> ().sprite = null;
			}
		}
		private void applyStack(List<Lite> pAppliedStacks)
		{
			
		}

		/// <summary>
		/// Update this instance of HUD observer
		/// </summary>
		/// <param name="newState">New state.</param>
		public void OnNext(PlayerState newState)
		{
			applyStack (newState.appliedStacks);
		}
	}
}