
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Class handling PulsePaths of the Daydream puzzle.
    /// PulsePaths have exactly one Pulse, and one PulseDestination,
    /// as well as a list of points to render its line with
    /// Though not shown here, a PulsePath may also have BlinkablePulseBars
    /// and ColorDepots to further affect its difficulty
	/// </summary>
    public class PulsePath : MonoBehaviour {

        Pulse pulse;
        Daydream daydream;
        List<Vector3> points;
        LineRenderer lineRenderer;

        public void activate(Daydream pDaydream)
        {
            daydream = pDaydream;

            // Activate Path (via its points)
            points = new List<Vector3>();
            populatePoints();
            lineRenderer = GetComponent<LineRenderer>();
            resetLineColoring(1f);
            updateLineRendererPositions();

            // Activate Pulse
            pulse = transform.Find("Pulse").GetComponent<Pulse>();
            pulse.activate(this);
            pulse.setColorProperties();

            activateColorDepots();
            activatePulseDestination();
        }

        public void deactivate()
        {
            deactivatePulseDestination();
            deactivateColorDepots();

            // // Deactivate Pulse
            pulse.deactivate();
            pulse = null;

            // // Deactivate Path
            lineRenderer.positionCount = points.Count;
            lineRenderer = null;
            points = null;

            daydream = null;
        }

        /// <summary>
        /// Called by Pulse when it reaches its destination with the correct
        /// coloration.
        /// </summary>
        public void close()
        {
            daydream.handleClosedPath(this);
            lineRenderer.material.color = Color.black;
        }

        /// <summary>
        /// Called by Pulse every time it reaches the end of its PulsePath
        /// whether or not it was properly colored.
        /// </summary>
        public void fullyTraversed()
        {
            daydream.updateTransitPulseCount(-1);
        }

        public List<Vector3> getPoints()
        {
            return points;
        }

        void populatePoints()
        {
            foreach (Transform point in transform.Find("Points"))
            {
                points.Add(point.position);
                point.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        void activateColorDepots()
        {
            foreach (Transform colorDepot in transform.Find("Depots"))
            {
                colorDepot.GetComponent<ColorDepot>().activate();
            }
        }

        void deactivateColorDepots()
        {
            foreach (Transform colorDepot in transform.Find("Depots"))
            {
                colorDepot.GetComponent<ColorDepot>().deactivate();
            }
        }

        void activatePulseDestination()
        {
            transform.Find("PulseDestination").GetComponent<PulseDestination>().activate();
        }

        void deactivatePulseDestination()
        {
             transform.Find("PulseDestination").GetComponent<PulseDestination>().deactivate();
        }

        public void firePulse()
        {
            pulse.fire();
        }

        public void resetLineColoring(float pAlpha)
        {
            lineRenderer.material.color = Color.white;
            var tempLineColor = lineRenderer.material.color;
            tempLineColor.a = pAlpha;
            lineRenderer.material.color = tempLineColor;
        }

        void updateLineRendererPositions()
		{
			lineRenderer.positionCount = points.Count;
			lineRenderer.SetPositions(points.ToArray());
		}
	}
}
