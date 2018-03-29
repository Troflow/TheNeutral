using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
    /// Attachable script that dynamically hides objects based on their BlinkableType,
    /// when the player's eyes are closed.
	/// </summary>
	public class BlinkableSplineLine: MonoBehaviour {

        LineRenderer lineRenderer;
        [SerializeField]
        private BlinkableType type;

		void Start () {
            lineRenderer = GetComponent<LineRenderer>();
		}


        /// <summary>
        /// Reveals a blinkLine when Player's eyes are closed. Hides
        /// when eyes are open.
        /// </summary>
        /// <param name="pPlayerEyesOpen"></param>
        void handleBlinkLineVisibility(bool pPlayerEyesOpen)
        {
            var lineColor = lineRenderer.material.color;

            if (pPlayerEyesOpen) lineColor.a = 0;
            else lineColor.a = 1;

            lineRenderer.material.color = lineColor;
        }

        /// <summary>
        /// Hides a sightLine when Player's eyes are closed. Reveals
        /// when eyes are open
        /// </summary>
        /// <param name="pPlayerEyesOpen"></param>
        void handleSightLineVisibility(bool pPlayerEyesOpen)
        {
            var lineColor = lineRenderer.material.color;

            if (pPlayerEyesOpen) lineColor.a = 1;
            else lineColor.a = 0;

            lineRenderer.material.color = lineColor;
        }

        void handlePlayerEyesState()
        {
            var playerEyesOpen = true;
            if (GameManager.playerBlinkState == BlinkState.EyesOpen) playerEyesOpen = true;
            else if (GameManager.playerBlinkState == BlinkState.EyesClosed) playerEyesOpen = false;

            switch(type)
            {
                case BlinkableType.BlinkSplineLine:
                    handleBlinkLineVisibility(playerEyesOpen);
                    break;

                case BlinkableType.SightSplineLine:
                    handleSightLineVisibility(playerEyesOpen);
                    break;
            }
        }

		void Update () {
            handlePlayerEyesState();
		}
	}
}