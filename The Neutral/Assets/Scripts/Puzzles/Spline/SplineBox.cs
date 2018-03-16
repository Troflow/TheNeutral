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

        Lite lite;
        SplineLine splineLine;
        [SerializeField]
        SplineBox sibling;
        [SerializeField]
        SplineBoxType type;

        public void activate(Transform pLinePrefab)
        {
            splineLine = Instantiate(pLinePrefab, transform.position, Quaternion.identity, transform).GetComponent<SplineLine>();
            splineLine.setOriginAndDestination(this, sibling);
            splineLine.setLineAttributes();
        }

        public SplineBoxType getType()
        {
            return type;
        }

        public SplineLine getSplineLine()
        {
            return splineLine;
        }

        public void setSibling(SplineBox pSibling)
        {
            sibling = pSibling;
        }
	}
}
