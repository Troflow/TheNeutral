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

		// For Debugging
		static Mural muralMostRecentlyActivated;
		ColorWheel colorWheel;

		[SerializeField]
		int height;
		[SerializeField]
		Lite lite;
		Color targetColor;
		CombatColor combatColor;
		List<CombatColor> colorBook;

		public bool isColoredCorrectly;

		public void activate()
		{
			colorWheel = transform.parent.GetComponent<ColorWheel>();
			targetColor = CombatColor.liteLookupTable[lite].color.Value;
			combatColor = CombatColor.liteLookupTable[Lite.BLACK];
			colorBook = new List<CombatColor>()
			{
				combatColor
			};
		}

		public void deactivate()
		{
			colorBook = null;
			combatColor = null;
			colorWheel = null;
		}

		void compareCombatColor(CombatColor pCombatColor)
		{
			if (pCombatColor.color.Value == targetColor)
			{
				isColoredCorrectly = true;
				colorWheel.halt();
			}
		}

		private void addColorToColorBookAndCheck(CombatColor pCombatColor)
		{
			colorBook.Add(pCombatColor);
			combatColor = computeColoringBookColor();
			compareCombatColor(combatColor);
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
		public void OnTriggerStay(Collider pCollider)
		{
			if (pCollider.CompareTag("Player-Sphere"))
			{
				var playerState = pCollider.GetComponentInParent<PlayerState>();
				var playerActionState = playerState.getPlayerActionState();

				// If Dashing, immediately compare Player color to Mural color
				if (playerActionState == PlayerActionState.Dashing && muralMostRecentlyActivated != this)
				{
					compareCombatColor(playerState.getCurrentCombatColor());
					muralMostRecentlyActivated = this;
				}

				// If Attacking, add Player color to Mural's color, then compare
				else if (playerActionState == PlayerActionState.Attacking && muralMostRecentlyActivated != this)
				{
					addColorToColorBookAndCheck(playerState.getCurrentCombatColor());
					muralMostRecentlyActivated = this;
				}
			}
		}
		#endregion
	}
}
