using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Color Ring class.
    /// After the Player has stood on the Ring's surface for a period of time
    /// The Player's color changes to match the color of the Ring
	/// </summary>
	public class ColorRing : MonoBehaviour {

		[SerializeField]
		private Lite lite;
        private CombatColor combatColor;

        private static ColorRing ringInContactWithPlayer;
        private bool isGrantingColor = false;
        private bool enteredNewRing = false;

        void Start()
        {
            combatColor = CombatColor.liteLookupTable[lite];
        }

		#region COLLISION HANDLING
        // NOTE: It must always be the case that the Player-ColorWheel-Collider
        // enters the successive color ring BEFORE it completely leaves a previous ring

		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag("Player-Sphere"))
			{
                ringInContactWithPlayer = this;
                PlayerState pState = col.GetComponentInParent<PlayerState>();

                pState.stopGrantingColor();
                pState.startGrantingColor(combatColor);
			}
		}

		public void OnTriggerExit(Collider col)
		{
			if (col.CompareTag ("Player-Sphere"))
			{
                PlayerState pState = col.GetComponentInParent<PlayerState>();
                if (pState.getIsBeingGrantedNewColor() != null && ringInContactWithPlayer == this)
                {
                    pState.stopGrantingColor();
                }

            }

		}
		#endregion
	}
}
