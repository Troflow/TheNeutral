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

        private Coroutine currentCoroutineRunning;
        private bool isGrantingColor = false;
        private bool enteredNewRing = false;

        void Start()
        {
            combatColor = CombatColor.liteLookupTable[lite];
        }

		private void startGrantingColor(PlayerState pState)
		{
            if (!isGrantingColor)
            {
                currentCoroutineRunning = StartCoroutine(grantColor(pState));
            }
		}

		private IEnumerator grantColor(PlayerState pState)
		{
            isGrantingColor = true;
            var pCombatColor = pState.getCurrentCombatColor();

            // If PlayerState's color matches  Ring's color, don't try transferring
            if (pCombatColor.color.Key == combatColor.color.Key)
            {
                isGrantingColor = false;
                yield return null;
            }
            else
            {
                pState.setIncomingTransferColor(combatColor);

                // Once colorTransferValue reaches above 1f, set PlayerState's color to the color of this Ring
                for (float transferVal = 0f; transferVal <= 1.1f; transferVal += Time.deltaTime/GameManager.colorTransferTimeStep)
                {
                    pState.setColorTransferValue(transferVal);

                    if (transferVal > 1f)
                    {
                        // Do this in for-loop to prevent the delay after yield return null
                        pState.setCurrentCombatColor(this.combatColor);
                        isGrantingColor = false;
                    }

                    yield return null;
                }
            }

		}

		#region COLLISION HANDLING
		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag("Player-ColorWheel-Collider"))
			{
                PlayerState pState = col.GetComponentInParent<PlayerState>();
                pState.setColorTransferValue(0f);

                startGrantingColor(pState);
			}
		}

		public void OnTriggerExit(Collider col)
		{
			if (col.CompareTag ("Player-ColorWheel-Collider"))
			{
                if (currentCoroutineRunning != null)
                {
                    PlayerState pState = col.GetComponentInParent<PlayerState>();
                    pState.setColorTransferValue(0f);

                    isGrantingColor = false;
                    StopCoroutine(currentCoroutineRunning);
                }

            }

		}
		#endregion
	}
}
