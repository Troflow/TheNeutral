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

        // State preventing color arithemtic being done unless
        // the ColorRing is activated
        bool willGrantColor = false;

        static ColorRing ringInContactWithPlayer;

        public void activate()
        {
            combatColor = CombatColor.liteLookupTable[lite];
            willGrantColor = true;
        }

        public void deactivate()
        {
            combatColor = null;
            willGrantColor = false;
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
