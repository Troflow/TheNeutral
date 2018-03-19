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
    /// On Start(), the SplineClass will pair SplineBox's according to their name. If an error is
    /// caught, the entire process is stopped and an exception raised.
    /// A blockade SplineBox does not allow any type of SplineLine whatsoever to pass through it.
    /// </summary>
	public class SplineBox : MonoBehaviour {

        [SerializeField]
        bool hasBeenActivatedBefore = false;
        bool splineLineConnected = false;

        Lite lite;
        [SerializeField]
        SplineLine splineLine;
        [SerializeField]
        SplineBox sibling;
        [SerializeField]
        SplineBoxType type;

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
