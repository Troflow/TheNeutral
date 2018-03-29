
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class Daydream : MonoBehaviour {

        List<Transform> openPaths;
        [SerializeField]
        int pulsesInTransit;
        bool isActivated = false;

        void activateDaydream()
        {
            if (isActivated) return;

            openPaths = new List<Transform>();
            populatePulsePaths();
        }

        void deactivateDaydream()
        {}

        public void handleClosedPath(PulsePath pPath)
        {
            if (openPaths.Contains(pPath.transform))
            {
                openPaths.Remove(pPath.transform);
            }
        }

        public void updateTransitPulseCount(int pVal)
        {
            pulsesInTransit += pVal;
            if (pulsesInTransit == 0) playBeat();
        }

        void populatePulsePaths()
        {
            foreach (Transform path in transform)
            {
                openPaths.Add(path);
                path.GetComponent<PulsePath>().activate(this);
            }
        }

        void completed()
        {}

        void playBeat()
        {
            if (openPaths.Count == 0)
            {
                completed();
                return;
            }

            pulsesInTransit = openPaths.Count;

            foreach (Transform path in openPaths)
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
                playBeat();
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
