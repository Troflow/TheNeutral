using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neutral
{
    public abstract class CombatColor : MonoBehaviour, ICombatColor
    {

        public static IDictionary<Color, CombatColor> colorLookupTable = new Dictionary<Color, CombatColor>(new ColorEqualityComparer());

        public static CombatColor operator + (CombatColor c1, CombatColor c2)
        {

            Color subtractColor;
            if (c1.color.Value.ToString() == c2.color.Value.ToString()) subtractColor = c1.color.Value;
            else subtractColor = c1.color.Value + c2.color.Value;
           
            if (!colorLookupTable.ContainsKey(subtractColor)) {
                print("NOT IN TABLE! - " + subtractColor.ToString());
                subtractColor.a = 1f;
                return new CombatColorMixed(subtractColor);
            }
            print("IN TABLE! - " + colorLookupTable[subtractColor].color.Value.ToString());
            return colorLookupTable[subtractColor];
        }

        //public abstract Color GetColor();

        public KeyValuePair<Lite, Color> color;


        protected void PopulateColorLookupTable()
        {
            colorLookupTable.Add(Color.green, new CombatColorGreen());
            colorLookupTable.Add(Color.red, new CombatColorRed());
            colorLookupTable.Add(Color.blue, new CombatColorBlue());
            //throw new NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {
            PopulateColorLookupTable();
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

    class ColorEqualityComparer : IEqualityComparer<Color>
    {
        public bool Equals(Color c1, Color c2)
        {
            return ((c1.r == c2.r) & (c1.g == c2.g) & (c1.b == c2.b));
        }

        public int GetHashCode(Color obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}

