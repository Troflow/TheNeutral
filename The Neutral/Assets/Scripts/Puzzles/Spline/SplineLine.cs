using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class SplineLine : MonoBehaviour {

        private SplineBox originBox;
        private SplineBox destinationBox;
        [SerializeField]
        private SplineLineType type;
        private List<Transform> occupiedTiles;

        private LineRenderer lineRenderer;

        /// <summary>
        /// Called by the SplineBox when Player is in contact with a SplineTile
        /// with a Standard SplineBox and performs a Tier
        /// </summary>
        /// <param name="pSplineBox"></param>
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

        /// <summary>
        /// Used by SplineTile to check if the next tile to add is too far away
        /// </summary>
        /// <returns></returns>
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
            lineRenderer = transform.GetComponent<LineRenderer>();
        }

        public void addOccupiedTile(Transform pTileTransform)
        {
            occupiedTiles.Add(pTileTransform);
        }

        /// <summary>
        /// Called by Spline when currentSplineLine is cleared
        /// </summary>
        public void clearAllTileOccupations()
        {
            foreach (Transform tile in occupiedTiles)
            {
                tile.gameObject.GetComponent<SplineTile>().clearOccupations();
            }
            occupiedTiles.Clear();
        }

        /// <summary>
        /// Called by SplineBox when it is activted. This is because either sibling
        /// can be the origin - depending on which SplineBox's tile that the player
        /// is in contact with when they perform the Tier
        /// </summary>
        /// <param name="pSplineBox"></param>
        /// <param name="pSplineBoxSibling"></param>
        public void setOriginAndDestination(SplineBox pSplineBox, SplineBox pSplineBoxSibling)
        {
            originBox = pSplineBox;
            destinationBox = pSplineBoxSibling;
        }

        /// <summary>
        /// Called by Spline and SplineBox for when currentSplineLine is cleared,
        /// or when the SplineBox is activated, respectively
        /// </summary>
        public void removeAllPoints()
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
        }
	}
}
