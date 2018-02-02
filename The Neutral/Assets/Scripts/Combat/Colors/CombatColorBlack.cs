using UnityEngine;
using System.Collections;


namespace Neutral
{

    public class CombatColorBlack : CombatColor
    {

        public CombatColorBlack()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.BLACK, Color.black);
        }


        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the Black color!");
        }

    }

}
