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

        private static ReverseMural muralInContactWithPlayer;

		void Start () {
			combatColor = CombatColor.liteLookupTable[Lite.BLACK];
		}

		#region Collision Handling
		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag ("Player-Sphere"))
			{
				var pState = col.GetComponentInParent<PlayerState>();
                //TODO: Immediately grant color to player rather than using a coroutine
			}
		}
		#endregion
	}
}
