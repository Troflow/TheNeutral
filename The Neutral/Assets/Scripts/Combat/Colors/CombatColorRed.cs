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
            if (colorLookupTable.Count() == 0)
            {
                PopulateColorLookupTable();
            }
            //print(color.Value.ToString());
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }


        public CombatColor TestSubtractColor()
        {
            List<Color> keys = colorLookupTable.Keys.ToList();
            int randInd = Random.Range(0, colorLookupTable.Count());
            CombatColor randomcColorFromDict = colorLookupTable[keys[randInd]];
            return (this + randomcColorFromDict);
        }
    }

}
