using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    /// <summary>
    /// Class handling boxes that appear in a Spline puzzle. There are two types of SplineBoxes
    /// Either a blockade or a standard.
    /// A standard SplineBox is one end of a SplineLine and has the naming convention:
    /// "SplineBox_[Color]_[Index]
    /// e.g. SplineBox_Yellow_0
    /// On activate(), the SplineClass will pair SplineBox's according to their name.
    /// A blockade SplineBox does not allow any type of SplineLine whatsoever to pass through it.
    /// </summary>
	public class SplineBox : MonoBehaviour {

        // Handles if a SplineLine prefab should be instantiated, or simply cleared for redrawing
        bool hasBeenActivatedBefore = false;

        // Bool allowing Spline to increment - or decrement - its completedSplineCount
        bool splineLineConnected = false;

        SplineLine splineLine;
        SplineBox sibling;
        [SerializeField]
        SplineBoxType type;

        /// <summary>
        /// This method is called by SplineTile when the player is in contact with
        /// the tile and performs a Tier.
        /// If never been activated, it will create the SplineLine object. Else,
        /// it will wipe the splineLine of all its points. If the line had been
        /// completed, the Spline will have its completedSplineCount decremented
        /// </summary>
        /// <param name="pLinePrefab"></param>
        /// <param name="pSpline"></param>
        public void activate(Transform pLinePrefab, Spline pSpline)
        {
            if (!hasBeenActivatedBefore)
            {
                splineLine = Instantiate(pLinePrefab, transform.position, Quaternion.identity, transform).GetComponent<SplineLine>();
                splineLine.activate(this);

                sibling.setSplineLine(splineLine);
                sibling.setHasBeenActivatedBefore(true);

                hasBeenActivatedBefore = true;
            }
            else
            {
                splineLine.removeAllPoints();
                splineLine.setOriginAndDestination(this, sibling);

                if (splineLineConnected)
                {
                    pSpline.reduceCompletedSplineCountBy(1);

                    splineLineConnected = false;
                    sibling.setSplineLineConnected(false);
                }
            }

        }

        /// <summary>
        /// Called by the SplineTile when the entire Spline puzzle is deactivated
        /// </summary>
        public void deactivate()
        {
            if (splineLine != null) Destroy(splineLine.gameObject);

            splineLine = null;
            hasBeenActivatedBefore = false;
            splineLineConnected = false;
            sibling = null;
        }

        public void setSplineLineConnected(bool pNewState)
        {
            splineLineConnected = pNewState;
        }

        public void setHasBeenActivatedBefore(bool pNewState)
        {
            hasBeenActivatedBefore = pNewState;
        }

        public SplineBoxType getType()
        {
            return type;
        }

        public SplineLine getSplineLine()
        {
            return splineLine;
        }

        public void setSplineLine(SplineLine pSplineLine)
        {
            splineLine = pSplineLine;
        }

        public void setSibling(SplineBox pSibling)
        {
            sibling = pSibling;
        }

        public SplineBox getSibling()
        {
            return sibling;
        }
	}
}
