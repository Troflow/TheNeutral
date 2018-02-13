using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class BodyBoxScoring : MonoBehaviour {

		void Start () {
		}

		/// <summary>
		/// Checks the scoring.
		/// </summary>
		public void checkScoring()
		{
			foreach (Transform child in transform)
			{
				if (!child.GetComponent<BodyBoxSwitch> ().isCoveredCorrectly)
					return;
			}
		}
	}
}
