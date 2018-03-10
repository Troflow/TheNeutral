using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Neutral
{

    public class CombatColorYellow : CombatColor
    {

        public CombatColorYellow()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.YELLOW, Color.yellow);
        }

        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the yellow color!");
        }

    }

}
