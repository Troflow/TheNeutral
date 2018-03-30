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
        CombatColor combatColor;
        List<CombatColor> colorBook;

        void Start()
        {
            combatColor = CombatColor.liteLookupTable[Lite.BLACK];
            colorBook = new List<CombatColor>()
            {
                combatColor
            };
        }

        public void addColorToColorBook(CombatColor pCombatColor)
        {
            var wheelAdded = transform.root.GetComponent<ReverseCarousel>().addNewlyColoredWheel(transform.parent);

            if (wheelAdded)
            {
                colorBook.Add(pCombatColor);
                combatColor = computeColoringBookColor();

                changeRingColor();
            }
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

        public void clearColoringBook()
        {
            combatColor = CombatColor.liteLookupTable[Lite.BLACK];
            colorBook.Clear();
            clearRingColor();
        }

        public CombatColor getCombatColor()
        {
            return combatColor;
        }

        void clearRingColor()
        {
            // For Debugging purposes
            var ring = transform.GetComponent<MeshRenderer>();
            ring.material.color = new Color(0.74f, 0.74f, 0.71f, 1f);
        }

        void changeRingColor()
        {
            // For Debugging purposes
            var ring = transform.GetComponent<MeshRenderer>();
            ring.material.color = combatColor.color.Value;
        }

		#region COLLISION HANDLING
		public void OnTriggerEnter(Collider pCollider)
		{
			if (pCollider.CompareTag("Player-Sphere"))
			{
                var playerState = pCollider.GetComponentInParent<PlayerState>();

                // TODO: Only if Player isAttacking:
                addColorToColorBook(playerState.getCurrentCombatColor());
			}
		}
		#endregion
	}
}
