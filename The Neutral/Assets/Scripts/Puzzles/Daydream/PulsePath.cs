
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class PulsePath : MonoBehaviour {

        LineRenderer lineRenderer;
        Pulse pulse;

        // For Debugging
        List<Vector3> points;

        public void activate()
        {
            pulse = transform.Find("Pulse").GetComponent<Pulse>();

            points = new List<Vector3>();
            populatePoints();

            lineRenderer = GetComponent<LineRenderer>();
            updateLineRendererPositions();
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
            pulse.setIsFired(true, points);
        }

        void updateLineRendererPositions()
		{
			lineRenderer.positionCount = points.Count;
			lineRenderer.SetPositions(points.ToArray());
		}
	}
}
