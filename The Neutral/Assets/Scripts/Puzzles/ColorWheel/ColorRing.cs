using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Color Ring class.
    /// After the Player has stood on the Ring's surface for a period of time
    /// the Player's color changes to match the color of the Ring
	/// </summary>
	public class ColorRing : MonoBehaviour {

		[SerializeField]
		Lite lite;
        CombatColor combatColor;
        bool willGrantColor = false;

        static ColorRing ringInContactWithPlayer;

        void Start()
        {
            combatColor = CombatColor.liteLookupTable[lite];
        }

        public void setWillGrantColor(bool pNewState)
        {
            willGrantColor = pNewState;
        }

		#region COLLISION HANDLING
		public void OnTriggerEnter(Collider pCollider)
		{
			if (willGrantColor && pCollider.CompareTag("Player-Sphere"))
			{
                ringInContactWithPlayer = this;
                var playerState = pCollider.GetComponentInParent<PlayerState>();

                playerState.stopGrantingColor();
                playerState.startGrantingColor(combatColor);
			}
		}

		public void OnTriggerExit(Collider pCollider)
		{
			if (willGrantColor && pCollider.CompareTag ("Player-Sphere"))
			{
                var playerState = pCollider.GetComponentInParent<PlayerState>();
                if (playerState.getIsBeingGrantedNewColor() != null && ringInContactWithPlayer == this)
                {
                    playerState.stopGrantingColor();
                }
            }

		}
		#endregion
	}
}
