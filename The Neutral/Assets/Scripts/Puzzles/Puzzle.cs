using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Abstract Puzzle class, ensuring that all puzzles
	/// follow the same structure to maintain consistency
	/// </summary>
	abstract public class Puzzle : MonoBehaviour {

		/// <summary>
		/// The color of the puzzle, used to track
		/// completion in each set of puzzles
		/// </summary>
		protected Lite puzzleColor;
		/// <summary>
		/// Memory rewarded to player after puzzle has
		/// been successfully completed
		/// </summary>
		protected Memory puzzleMemory;

		protected abstract void puzzleCompleted();

	}
}
