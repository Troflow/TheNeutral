using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class SplineBox : MonoBehaviour {

        private Lite lite;
        [SerializeField]
        private SplineLine splineLine;

        public void activate(Transform pLinePrefab)
        {
            splineLine = Instantiate(pLinePrefab, transform.position, Quaternion.identity).GetComponent<SplineLine>();
            splineLine.setOriginBox(this);
            splineLine.setLineAttributes();

        }

        public SplineLine getSplineLine()
        {
            return splineLine;
        }
	}
}
