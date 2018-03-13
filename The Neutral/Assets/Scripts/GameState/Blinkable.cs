using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Neutral
{
	/// <summary>
	/// </summary>
	public class Blinkable : MonoBehaviour {

        UnityEngine.AI.NavMeshObstacle obstacle;
        MeshRenderer renderer;
        public BlinkableType type;

        // Use this for initialization
		void Start () {
            obstacle = GetComponent<NavMeshObstacle>();
            renderer = GetComponent<MeshRenderer>();
		}


        void standardHandle(bool pPlayerEyesOpen)
        {
            obstacle.enabled = pPlayerEyesOpen;
            renderer.enabled = pPlayerEyesOpen;
        }

        void handlePlayerEyesState(bool pPlayerEyesOpen)
        {
            switch(type)
            {
                case BlinkableType.Triggerable:
                    break;

                case BlinkableType.NonTriggerable:
                    standardHandle(pPlayerEyesOpen);
                    break;

                case BlinkableType.Enemy:
                    break;
            }
        }

		// Update is called once per frame
		void Update () {
            handlePlayerEyesState(GameManager.playerEyesOpen);
		}
	}
}