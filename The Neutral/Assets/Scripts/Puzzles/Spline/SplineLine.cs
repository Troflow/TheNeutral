using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class SplineLine : MonoBehaviour {

        private bool isCompleted = false;
        private SplineBox originBox;
        private SplineBox destinationBox;
        [SerializeField]
        private SplineLineType type;
        private List<Transform> occupiedTiles;

        // private Lite lite;

        private Transform lineObject;
        private LineRenderer lineRenderer;

        public void activate(SplineBox pSplineBox)
        {
            occupiedTiles = new List<Transform>();
            setOriginAndDestination(pSplineBox, pSplineBox.getSibling());
            setLineAttributes();
        }
        public SplineLineType getType()
        {
            return type;
        }

        public LineRenderer getLineRenderer()
        {
            return lineRenderer;
        }

        public SplineBox getDestination()
        {
            return destinationBox;
        }

        public Transform getLastOccupiedTile()
        {
            if (occupiedTiles != null)
            {
                return occupiedTiles[occupiedTiles.Count - 1];
            }

            else return null;
        }

        public void setLineAttributes()
        {
            lineObject = transform;
            lineRenderer = transform.GetComponent<LineRenderer>();
        }

        public void addOccupiedTile(Transform pTileTransform)
        {
            occupiedTiles.Add(pTileTransform);
        }

        public void clearAllTileOccupations()
        {
            foreach (Transform tile in occupiedTiles)
            {
                tile.gameObject.GetComponent<SplineTile>().clearOccupations();
            }
            occupiedTiles.Clear();
        }

        public void setOriginAndDestination(SplineBox pSplineBox, SplineBox pSplineBoxSibling)
        {
            originBox = pSplineBox;
            destinationBox = pSplineBoxSibling;
        }

        public void removeAllPoints()
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
        }
	}
}
