using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Neutral
{

    public class CombatColorMagenta : CombatColor
    {

        public CombatColorMagenta()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.MAGENTA, Color.magenta);
        }

        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the magenta color!");
        }

    }

}
