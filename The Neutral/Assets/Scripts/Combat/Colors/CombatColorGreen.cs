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

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
