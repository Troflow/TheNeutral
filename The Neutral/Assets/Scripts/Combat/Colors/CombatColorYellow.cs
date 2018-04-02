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
            var customYellow = new Color(1,1,0,1);
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.YELLOW, customYellow);
        }

        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the yellow color!");
        }

    }

}
