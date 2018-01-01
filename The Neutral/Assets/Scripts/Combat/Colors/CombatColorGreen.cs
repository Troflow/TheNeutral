using UnityEngine;
using System.Collections;


namespace Neutral
{

    public class CombatColorGreen : CombatColor
    {
        public CombatColorGreen()
        { 
            color = new System.Collections.Generic.KeyValuePair<Lite, Color>(Lite.GREEN, Color.green);
        }


        public override void ExposedColorLogic()
        {
            Debug.Log("This is custom logic for the green color!");
        }

    }

}
