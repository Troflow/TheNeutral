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
        public Lite testLite;
        private CombatColor combatColor;
        private List<CombatColor> colorBook;

        void Start()
        {
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
            testLite = combatColor.color.Key;
            transform.root.GetComponent<ReverseCarousel>().addNewlyColoredWheel(transform.parent);

            changeRingColor();
        }

        private void changeRingColor()
        {
            // For Debugging purposes
            MeshRenderer ring = transform.GetComponent<MeshRenderer>();
            ring.material.color = combatColor.color.Value;
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

        public CombatColor getCombatColor()
        {
            return combatColor;
        }

		#region COLLISION HANDLING
		public void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag("Player-Sphere"))
			{
                var pState = col.GetComponentInParent<PlayerState>();

                // If Player isAttacking:
                addColorToColorBook(pState.getCurrentCombatColor());
			}
		}
		#endregion
	}
}
