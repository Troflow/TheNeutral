using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Neutral
{

    public class CombatColorGrey : CombatColor
    {

        public CombatColorGrey()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.GREY, Color.grey);
        }

        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the grey color!");
        }

    }

}
