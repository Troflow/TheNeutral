using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ReverseMural class.
	/// </summary>
	public class ReverseMural : MonoBehaviour {
        [SerializeField]
		Lite lite;
		CombatColor combatColor;

		public void Activate()
		{
			combatColor = CombatColor.liteLookupTable[lite];
		}

		public void Deactivate()
		{
			combatColor = null;
		}

		public CombatColor getCombatColor()
		{
			return combatColor;
		}

		#region Collision Handling
		public void OnTriggerEnter(Collider pCollider)
		{
			if (pCollider.CompareTag ("Player-Sphere"))
			{
				var playerState = pCollider.GetComponentInParent<PlayerState>();
				playerState.setCurrentCombatColor(combatColor);
			}
		}
		#endregion
	}
}
