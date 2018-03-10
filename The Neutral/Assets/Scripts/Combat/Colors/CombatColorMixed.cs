using UnityEngine;
using System.Collections;


namespace Neutral
{

    public class CombatColorMixed : CombatColor
    {

        public CombatColorMixed(Color _color)
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.MIXED, _color);
        }


        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the mixed color!");
        }

    }

}
