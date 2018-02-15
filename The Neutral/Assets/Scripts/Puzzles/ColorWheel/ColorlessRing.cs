using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	/// <summary>
	/// ColorlessRing class.
    /// After the Player has stood on the Ring's surface for a period of time
    /// The Player's color changes to match the color of the Ring
	/// </summary>
	public class ColorlessRing : MonoBehaviour {
        private ColorlessWheel colorlessWheel;

		[SerializeField]
		private Lite lite;
        private CombatColor combatColor;
        private List<CombatColor> colorBook;

        void Start()
        {
            colorlessWheel = transform.parent.GetComponent<ColorlessWheel>();
            combatColor = CombatColor.liteLookupTable[Lite.BLACK];
            colorBook = new List<CombatColor>()
            {
                combatColor
            };
        }

        public void addColorToColorBook(CombatColor color)
        {
            colorBook.Add(color);
            combatColor = computeColoringBookColor();
            colorlessWheel.getNewColor(combatColor);
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

		#region COLLISION HANDLING
		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag("Player-Sphere"))
			{
                var pState = col.GetComponentInParent<PlayerState>();
                addColorToColorBook(pState.getCurrentCombatColor());
			}
		}
		#endregion
	}
}
