using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Neutral
{

    public class CombatColorWhite : CombatColor
    {

        public CombatColorWhite()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.WHITE, Color.white);
        }

        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the white color!");
        }

    }

}
