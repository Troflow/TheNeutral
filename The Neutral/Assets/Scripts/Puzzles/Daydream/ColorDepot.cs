using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class ColorDepot : MonoBehaviour {
        [SerializeField]
        Lite lite;
        CombatColor combatColor;
        MeshRenderer meshRenderer;

        public void activate()
        {
            combatColor = CombatColor.liteLookupTable[lite];
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = combatColor.color.Value;
        }

        public void deactivate()
        {
            combatColor = CombatColor.liteLookupTable[Lite.WHITE];
            meshRenderer.material.color = combatColor.color.Value;

            combatColor = null;
            meshRenderer = null;
        }

        public CombatColor getCombatColor()
        {
            return combatColor;
        }
	}
}
