using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Neutral
{
	/// <summary>
    /// Attachable script that dynamically hides objects based on their BlinkableType,
    /// when the player's eyes are closed.
	/// </summary>
	public class Blinkable : MonoBehaviour {

        UnityEngine.AI.NavMeshObstacle obstacle;
        MeshRenderer renderer;
        public BlinkableType type;

		void Start () {
            obstacle = GetComponent<NavMeshObstacle>();
            renderer = GetComponent<MeshRenderer>();
		}


        void standardHandle(bool pPlayerEyesOpen)
        {
            obstacle.enabled = pPlayerEyesOpen;
            renderer.enabled = pPlayerEyesOpen;
        }

        void handlePlayerEyesState(BlinkState pPlayerBlinkState)
        {
            var playerEyesOpen = true;
            if (pPlayerBlinkState == BlinkState.EyesOpen) playerEyesOpen = true;
            else if (pPlayerBlinkState == BlinkState.EyesClosed) playerEyesOpen = false;

            switch(type)
            {
                case BlinkableType.Triggerable:
                    break;

                case BlinkableType.NonTriggerable:
                    standardHandle(playerEyesOpen);
                    break;

                case BlinkableType.Enemy:
                    break;
            }
        }

		void Update () {
            handlePlayerEyesState(GameManager.playerBlinkState);
		}
	}
}