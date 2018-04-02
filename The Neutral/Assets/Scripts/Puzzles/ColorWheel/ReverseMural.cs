using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ReverseMural class.
	/// </summary>
	public class ReverseMural : MonoBehaviour {
        // For Debugging
		static ReverseMural muralMostRecentlyActivated;

		[SerializeField]
		Lite lite;
		CombatColor combatColor;

		public void activate()
		{
			combatColor = CombatColor.liteLookupTable[lite];
		}

		public void deactivate()
		{
			combatColor = null;
		}

		public CombatColor getCombatColor()
		{
			return combatColor;
		}

		#region Collision Handling
		public void OnTriggerStay(Collider pCollider)
		{
			if (pCollider.CompareTag ("Player-Sphere"))
			{
				var playerState = pCollider.GetComponentInParent<PlayerState>();
				var playerActionState = playerState.getPlayerActionState();

				if (playerActionState == PlayerActionState.Dashing &&
					muralMostRecentlyActivated != this)
				{
					playerState.setCurrentCombatColor(combatColor);
					muralMostRecentlyActivated = this;
				}
			}
		}
		#endregion
	}
}
