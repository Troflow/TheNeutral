
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class Daydream : MonoBehaviour {

        List<Transform> pulsePaths;
        bool isActivated = false;

        void activateDaydream()
        {
            if (isActivated) return;

            pulsePaths = new List<Transform>();
            populatePulsePaths();

            foreach (Transform path in pulsePaths)
            {
                path.GetComponent<PulsePath>().activate();
            }
        }

        void deactivateDaydream()
        {}

        void populatePulsePaths()
        {
            foreach (Transform child in transform)
            {
                pulsePaths.Add(child);
            }
        }

        void playBeat()
        {
            foreach (Transform path in pulsePaths)
            {
                path.GetComponent<PulsePath>().firePulse();
            }
        }

        // For Debugging
		void handleInput()
        {
            if (Input.GetKeyDown(KeyCode.Comma))
			{
				Debug.Log("Comma Pressed");
				activateDaydream();
                InvokeRepeating("playBeat", 0, 4f);
			}

			if (Input.GetKeyDown (KeyCode.Period))
			{
				Debug.Log("Period Pressed");
				deactivateDaydream();
			}
        }

        void Update()
        {
            handleInput();
        }
	}
}
