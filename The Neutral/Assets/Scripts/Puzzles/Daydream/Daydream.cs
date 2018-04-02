
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Class handling the Daydream puzzle. Daydreams contain a number of PulsePaths
    /// that must be 'closed' in order to complete the puzzle.
    /// Pulses must have the same color as their deestination in order to close the path
    /// that the pulse is on
	/// </summary>
    public class Daydream : MonoBehaviour {

        bool isCompleted = false;
        int pulsesInTransit;
        bool isActivated = false;

        List<Transform> openPaths;

        void activateDaydream()
        {
            // Do nothing if already activated or completed
            if (isActivated || isCompleted) return;

            openPaths = new List<Transform>();
            populateOpenPulsePaths();
            isActivated = true;
        }

        void deactivateDaydream()
        {
            // Do nothing if already deactivated
            if (!isActivated) return;

            isActivated = false;
            pulsesInTransit = 0;

            foreach (Transform path in transform)
            {
                var pulsePath = path.GetComponent<PulsePath>();

                // Reset path colorations, if puzzle wasn't completed before deactivation
                if (!isCompleted) pulsePath.resetLineColoring(0f);

                pulsePath.deactivate();
            }
        }

        /// <summary>
        /// Adds each child (a PulsePath) of the Daydream to its openPaths
        /// attribute, and calls its activate() method
        /// </summary>
        void populateOpenPulsePaths()
        {
            foreach (Transform path in transform)
            {
                openPaths.Add(path);
                path.GetComponent<PulsePath>().activate(this);
            }
        }

        /// <summary>
        /// Called by PulsePath when its Pulse has successfully reached its destination
        /// with the correct coloration
        /// </summary>
        /// <param name="pPath"></param>
        public void handleClosedPath(PulsePath pPath)
        {
            if (openPaths.Contains(pPath.transform))
            {
                openPaths.Remove(pPath.transform);
            }
        }

        /// <summary>
        /// Decrements the pulsesInTransit count by the given pVal, and if
        /// the pulsesInTransit reaches 0, starts the next beat immediately
        /// </summary>
        /// <param name="pVal"></param>
        public void updateTransitPulseCount(int pVal)
        {
            pulsesInTransit += pVal;
            if (pulsesInTransit == 0) playBeat();
        }

        void completed()
        {
            isCompleted = true;
            deactivateDaydream();
        }

        /// <summary>
        /// A 'Beat' is played every time all Pulses that were previously fired
        /// reach the end of their Path.
        /// If no Paths are left open, the puzzle is considered completed
        /// </summary>
        void playBeat()
        {
            if (!isActivated) return;

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
