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
	public class BlinkableObstacle : MonoBehaviour, IBlinkable {

        UnityEngine.AI.NavMeshObstacle obstacle;
        MeshRenderer renderer;
        [SerializeField]
        BlinkableType type;

		void Start()
        {
            obstacle = GetComponent<NavMeshObstacle>();
            renderer = GetComponent<MeshRenderer>();
		}


        void standardHandle(bool pPlayerEyesOpen)
        {
            obstacle.enabled = pPlayerEyesOpen;
            renderer.enabled = pPlayerEyesOpen;
        }

        public void handlePlayerEyesState()
        {
            var playerEyesOpen = true;
            if (GameManager.playerBlinkState == BlinkState.EyesOpen) playerEyesOpen = true;
            else if (GameManager.playerBlinkState == BlinkState.EyesClosed) playerEyesOpen = false;

            switch(type)
            {
                case BlinkableType.TriggerableObstacle:
                    break;

                case BlinkableType.NonTriggerableObstacle:
                    standardHandle(playerEyesOpen);
                    break;

                case BlinkableType.Enemy:
                    break;
            }
        }

		void Update()
        {
            handlePlayerEyesState();
		}
	}
}