using UnityEngine;
using System.Collections;


namespace Neutral
{

    public class CombatColorBlue : CombatColor
    {

        public CombatColorBlue()
        {
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.BLUE, Color.blue);
        }


        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the Blue color!");
        }

    }

}
