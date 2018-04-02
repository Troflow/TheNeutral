using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class PulseDestination : MonoBehaviour {

        [SerializeField]
        Lite lite;
        MeshRenderer meshRenderer;
        bool isClosed = false;

        public void activate()
        {
            var combatColor = CombatColor.liteLookupTable[lite];
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = combatColor.color.Value;
        }

        public void deactivate()
        {
            var combatColor = CombatColor.liteLookupTable[Lite.WHITE];
            meshRenderer.material.color = combatColor.color.Value;

            isClosed = false;
            meshRenderer = null;
        }

        public bool getIsClosed()
        {
            return isClosed;
        }

        public void setIsClosed(bool pNewState)
        {
            isClosed = pNewState;
        }

        public Lite getLite()
        {
            return lite;
        }
	}
}