using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Color Ring class, responsible for granting the player
	/// the designated ringColorgame after staying in contact with
	/// the player for a given amount of time.
	/// </summary>
	public class ColorRing : MonoBehaviour {

        [SerializeField]
		private float colorTransferTime;
		[SerializeField]
		private Lite ringColor;

        private Coroutine currentCoroutineRunning;
        private bool isGrantingColor;

        void Start()
        {
            isGrantingColor = false;
        }

        public void setTransferTime(float pTransferTime)
		{
			colorTransferTime = pTransferTime;
		}

		private void startGrantingColor(PlayerState pState)
		{
            if (!isGrantingColor)
            {
                //Debug.Log("STARTING COROUTINE");
                currentCoroutineRunning = StartCoroutine(grantColor(pState));
            }
		}

		//TODO: Hussain: timing isn't working correctly. Method isn't always waiting the
		// given amount
		private IEnumerator grantColor(PlayerState pState)
		{
            isGrantingColor = true;
            if (ringColor == pState.heldColor)
            {
                pState.stopPulseFlag();
                isGrantingColor = false;
                yield return null;
            }
            else
            {
                pState.pulseFlag(ringColor, colorTransferTime);

                yield return new WaitForSeconds(colorTransferTime);
                Debug.Log("FINISHED WAITFORTIME!");
                pState.heldColor = ringColor;
                pState.stopPulseFlag();
                isGrantingColor = false;
            }

		}

		#region Collision Handling
		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag("Player-Sphere")) 
			{
                PlayerState pState = col.GetComponentInParent<PlayerState>();
                pState.stopPulseFlag();
                startGrantingColor(pState);
			}
		}

		public void OnTriggerExit(Collider col)
		{
			if (col.CompareTag ("Player-Sphere")) 
			{
                //Debug.Log("exited ring color: "+ringColor);
                if (currentCoroutineRunning != null)
                {
                    StopCoroutine(currentCoroutineRunning);
                    // StopAllCoroutines();
                    isGrantingColor = false;
                    //Debug.Log("STOPPING COROUTINES");
                }

            }

		}
		#endregion
	}
}
