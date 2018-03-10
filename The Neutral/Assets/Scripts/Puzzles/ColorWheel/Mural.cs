using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// Mural class.
	/// The objective is to get either the Mural's combatColor
	/// or the Player's currentCombatColor to match
	/// with the Mural's Lite
	/// </summary>
	public class Mural : MonoBehaviour {

		ColorWheel colorWheel;

		[SerializeField]
		int height;
		[SerializeField]
		Lite lite;
		List<CombatColor> colorBook;
		CombatColor combatColor;

		public bool isColoredCorrectly;

		void Start () {
			colorWheel = transform.parent.GetComponent<ColorWheel>();
			combatColor = CombatColor.liteLookupTable[Lite.BLACK];
			colorBook = new List<CombatColor>()
			{
				combatColor
			};
		}

		void checkColorMatchesLite(CombatColor pCombatColor)
		{
			if (pCombatColor.color.Key == lite)
			{
				isColoredCorrectly = true;
				colorWheel.halt();
			}
		}

		private void addColorToColorBookAndCheck(CombatColor pCombatColor)
		{
			colorBook.Add(pCombatColor);
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

		public int getHeight()
		{
			return height;
		}

		#region Collision Handling
		public void OnTriggerEnter(Collider pCollider)
		{
			if (pCollider.CompareTag ("Player-Sphere"))
			{
				var playerState = pCollider.GetComponentInParent<PlayerState>();

				// TODO:
				// If Player isDashing, simply check if Player's currentCombatColor matches the mural's combatColor
				// if (playerState.isDashing):
				checkColorMatchesLite(playerState.getCurrentCombatColor());

				// TODO:
				// Else, if Player isAttacking, add the Player's currentCombatColor to the mural's combatColor,
				// then check if the Mural's combatColor.color.Key matches its lite
				addColorToColorBookAndCheck(playerState.getCurrentCombatColor());
			}
		}
		#endregion
	}
}
