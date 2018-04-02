using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
    /// Attachable script that dynamically hides objects based on their BlinkableType,
    /// when the player's eyes are closed.
	/// </summary>
	public class BlinkablePulseBar: MonoBehaviour, IBlinkable {

        MeshRenderer meshRenderer;
        Collider collider;
        [SerializeField]
        BlinkableType type;

		void Start () {
            meshRenderer = GetComponent<MeshRenderer>();
            collider = GetComponent<Collider>();
		}


        void handleBlinkPulseBarVisibility(bool pPlayerEyesOpen)
        {
            var barColor = meshRenderer.material.color;
            collider.enabled = !pPlayerEyesOpen;

            if (pPlayerEyesOpen)
            {
                barColor.a = 0;
            }
            else
            {
                barColor.a = 1;
            }

            meshRenderer.material.color = barColor;
        }

        void handleSightPulseBarVisibility(bool pPlayerEyesOpen)
        {
            var barColor = meshRenderer.material.color;
            collider.enabled = pPlayerEyesOpen;

            if (pPlayerEyesOpen)
            {
                barColor.a = 1;
            }
            else
            {
                barColor.a = 0;
            }

            meshRenderer.material.color = barColor;
        }

        public void handlePlayerEyesState()
        {
            var playerEyesOpen = true;
            if (GameManager.playerBlinkState == BlinkState.EyesOpen) playerEyesOpen = true;
            else if (GameManager.playerBlinkState == BlinkState.EyesClosed) playerEyesOpen = false;

            switch(type)
            {
                case BlinkableType.BlinkPulseBar:
                    handleBlinkPulseBarVisibility(playerEyesOpen);
                    break;

                case BlinkableType.SightPulseBar:
                    handleSightPulseBarVisibility(playerEyesOpen);
                    break;
            }
        }

		void Update () {
            handlePlayerEyesState();
		}
	}
}