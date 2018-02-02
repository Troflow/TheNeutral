using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Mural class.
	/// The objective is to get either the Mural's combatColor.color.Key
	/// or the Player's currentCombatColor.color.Key to match
	/// with the Mural's Lite
	/// </summary>
	public class Mural : MonoBehaviour {

		private ColorWheel colorWheel;

		[SerializeField]
		private Lite lite;
		public bool isColoredCorrectly;

		private List<CombatColor> colorBook;
		private CombatColor combatColor;

		void Start () {
			colorWheel = transform.parent.GetComponent<ColorWheel> ();
			combatColor = CombatColor.liteLookupTable[Lite.BLACK];
			colorBook = new List<CombatColor>()
			{
				combatColor
			};
		}

		private void checkColorMatchesLite(CombatColor pCombatColor)
		{
			if (pCombatColor.color.Key == lite)
			{
				isColoredCorrectly = true;
				colorWheel.halt();
			}
		}

		private void addColorToColorBookAndCheck(CombatColor color)
		{
			colorBook.Add(color);
			combatColor = computeColoringBookColor();
			checkColorMatchesLite(combatColor);
		}

		private CombatColor computeColoringBookColor()
		{
			var mixedColor = colorBook[0];
			for (int x = 1; x < colorBook.Count; x++)
			{
				mixedColor += colorBook[x];
			}

			return mixedColor;
		}

		#region Collision Handling
		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag ("Player-Sphere"))
			{
				var playerState = col.GetComponentInParent<PlayerState>();

				// If Player isDashing, simply check if Player's currentCombatColor matches the mural's combatColor
				// if (playerState.isDashing):
				checkColorMatchesLite(playerState.getCurrentCombatColor());

				// Else, if Player isAttacking, add the Player's currentCombatColor to the mural's combatColor,
				// then check if the Mural's combatColor.color.Key matches its lite
				// if (playerState.isAttacking):
				addColorToColorBookAndCheck(playerState.getCurrentCombatColor());
			}
		}
		#endregion
	}
}
