using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class BodyBoxScoring : Puzzle {

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
			puzzleCompleted ();
		}

		protected override void puzzleCompleted ()
		{
			throw new System.NotImplementedException ();
		}
	}
}
