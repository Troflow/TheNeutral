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
