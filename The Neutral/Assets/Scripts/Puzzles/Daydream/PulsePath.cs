
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class PulsePath : MonoBehaviour {

        Daydream daydream;
        LineRenderer lineRenderer;
        Pulse pulse;

        // For Debugging
        List<Vector3> points;

        public void activate(Daydream pDaydream)
        {
            daydream = pDaydream;
            pulse = transform.Find("Pulse").GetComponent<Pulse>();
            pulse.setPath(this);
            pulse.setColorProperties();

            points = new List<Vector3>();
            populatePoints();

            lineRenderer = GetComponent<LineRenderer>();
            updateLineRendererPositions();
        }

        public void deactivate()
        {}

        public void close()
        {
            daydream.handleClosedPath(this);
        }

        public void fullyTraversed()
        {
            daydream.updateTransitPulseCount(-1);
        }

        void populatePoints()
        {
            var pointsChild = transform.Find("Points");

            foreach (Transform point in pointsChild)
            {
                points.Add(point.position);
                var renderer = point.GetComponent<MeshRenderer>();
                renderer.enabled = false;
            }
        }

        public void firePulse()
        {
            pulse.fire(true, points);
        }

        void updateLineRendererPositions()
		{
			lineRenderer.positionCount = points.Count;
			lineRenderer.SetPositions(points.ToArray());
		}
	}
}
