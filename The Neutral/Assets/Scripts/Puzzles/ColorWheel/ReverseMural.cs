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
		private Lite lite;
		private CombatColor combatColor;

		void Start () {
			combatColor = CombatColor.liteLookupTable[lite];
		}

		public CombatColor getCombatColor()
		{
			return combatColor;
		}

		#region Collision Handling
		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag ("Player-Sphere"))
			{
				var pState = col.GetComponentInParent<PlayerState>();
				pState.setCurrentCombatColor(combatColor);
			}
		}
		#endregion
	}
}
