using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
	// All HUDElements must be added to the Script Execution Order to prevent
	// NullReferenceExeption Errors
	public class ActionStateIndicator : HUDElement, IObserver<PlayerState>
	{
		private Transform tierStateIndicator;
		private Transform counterStateIndicator;
        private Transform exploitStateIndicator;
        private Transform dashStateIndicator;

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

		void updateActionStateIndicator(PlayerActionState pActionState)
		{
            deactivateAllChildren();

            if (pActionState == PlayerActionState.Attacking)
            {
                tierStateIndicator.gameObject.SetActive(true);
            }
			else if (pActionState == PlayerActionState.CounterState)
            {
                counterStateIndicator.gameObject.SetActive(true);
            }
			else if (pActionState == PlayerActionState.Exploiting)
            {
                exploitStateIndicator.gameObject.SetActive(true);
            }
			else if (pActionState == PlayerActionState.Dashing)
            {
                dashStateIndicator.gameObject.SetActive(true);
            }
		}

		void getChildren()
		{
            tierStateIndicator = transform.Find("AttackState");
            counterStateIndicator = transform.Find("CounterState");
            exploitStateIndicator = transform.Find("ExploitState");
            dashStateIndicator = transform.Find("DashState");
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
			var actionState = newState.getPlayerActionState();
			updateActionStateIndicator(actionState);
		}
	}
}