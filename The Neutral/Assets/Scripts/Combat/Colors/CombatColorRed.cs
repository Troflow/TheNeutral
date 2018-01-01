using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Neutral
{

    public class CombatColorRed : CombatColor
    {

        public CombatColorRed()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.RED, Color.red);
        }


        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the red color!");
        }
    }

}
