using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class SplineLine : MonoBehaviour {

        private bool isCompleted = false;
        private SplineBox originBox;
        private SplineBox destinationBox;
        // private Lite lite;

        private Transform lineObject;
        private LineRenderer lineRenderer;

        public LineRenderer getLineRenderer()
        {
            return lineRenderer;
        }

        public void setLineAttributes()
        {
            lineObject = transform;
            lineRenderer = transform.GetComponent<LineRenderer>();
        }

        public void setOriginBox(SplineBox pSplineBox)
        {
            originBox = pSplineBox;
        }

        public void setDestination(SplineBox pSplineBox)
        {
            destinationBox = pSplineBox;
        }

        public void removeAllPoints()
        {
            if (!lineRenderer == null) lineRenderer.positionCount = 0;
        }
	}
}
