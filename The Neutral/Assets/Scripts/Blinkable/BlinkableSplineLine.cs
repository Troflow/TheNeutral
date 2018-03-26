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


        void handleBlinkLineVisibility(bool pPlayerEyesOpen)
        {
            var lineColor = lineRenderer.material.color;

            if (pPlayerEyesOpen) lineColor.a = 0;
            else lineColor.a = 1;

            lineRenderer.material.color = lineColor;
        }

        void handleSightLineVisibility(bool pPlayerEyesOpen)
        {

            var lineColor = lineRenderer.material.color;

            if (pPlayerEyesOpen) lineColor.a = 1;
            else lineColor.a = 0;

            lineRenderer.material.color = lineColor;
        }

        void handlePlayerEyesState(BlinkState pPlayerBlinkState)
        {
            var playerEyesOpen = true;
            if (pPlayerBlinkState == BlinkState.EyesOpen) playerEyesOpen = true;
            else if (pPlayerBlinkState == BlinkState.EyesClosed) playerEyesOpen = false;

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
            handlePlayerEyesState(GameManager.playerBlinkState);
		}
	}
}