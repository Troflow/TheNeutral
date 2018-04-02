using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Neutral
{

    public class CombatColorCyan : CombatColor
    {

        public CombatColorCyan()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.CYAN, Color.cyan);
        }

        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the cyan color!");
        }

    }

}
