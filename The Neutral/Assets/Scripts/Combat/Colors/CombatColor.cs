using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Neutral
{
    public abstract class CombatColor : ICombatColor
    {

        //when exploited/countered add a variable that is exploited/coutered which adds a lighter/darker shade to the current color

        public static IDictionary<Color, CombatColor> colorLookupTable = new Dictionary<Color, CombatColor>(new ColorEqualityComparer())
        {
            { Color.green, new CombatColorGreen() },
            { Color.red, new CombatColorRed() },
            { Color.blue, new CombatColorBlue() },
            { Color.yellow, new CombatColorYellow() },
            { Color.black, new CombatColorBlack() },
        };

        public static IDictionary<Lite, CombatColor> liteLookupTable = new Dictionary<Lite, CombatColor>()
        {
            { Lite.RED, new CombatColorRed() },
            { Lite.YELLOW, new CombatColorYellow() },
            { Lite.GREEN, new CombatColorGreen() },
            { Lite.CYAN, new CombatColorCyan() },
            { Lite.BLUE, new CombatColorBlue() },
            { Lite.MAGENTA, new CombatColorMagenta() },
            { Lite.GREY, new CombatColorGrey() },
            { Lite.BLACK, new CombatColorBlack() },
            { Lite.WHITE, new CombatColorWhite() },
        };

        public static CombatColor operator + (CombatColor c1, CombatColor c2)
        {
            Color newColor = Color.black;
            if (c1.color.Value.ToString() != c2.color.Value.ToString())
            {
                newColor.r = (float)((int)Math.Round(c2.color.Value.r) ^ (int)Math.Round(c1.color.Value.r));
                newColor.g = (float)((int)Math.Round(c2.color.Value.g) ^ (int)Math.Round(c1.color.Value.g));
                newColor.b = (float)((int)Math.Round(c2.color.Value.b) ^ (int)Math.Round(c1.color.Value.b));
            }

            else newColor = c1.color.Value;

            //Debug.Log("From adding " + c1.color.Value + " and " + c2.color.Value + " we get: " + newColor);

            if (!colorLookupTable.ContainsKey(newColor)) {
                newColor.a = 1f;
                //Debug.Log("NOT IN TABLE! - " + newColor.ToString());
                return new CombatColorMixed(newColor);
            }
            // Debug.Log("IN TABLE! - " + colorLookupTable[newColor].color.Value.ToString());

            return colorLookupTable[newColor];
        }

        public KeyValuePair<Lite, Color> color;


        public CombatColor TestSubtractColor(CombatColor currentColor)
        {
            List<Color> keys = colorLookupTable.Keys.ToList();
            int randInd = UnityEngine.Random.Range(0, colorLookupTable.Count());
            CombatColor randomColorFromDict = colorLookupTable[keys[randInd]];
            while (randomColorFromDict.color.Key == currentColor.color.Key)
            {
                randInd = UnityEngine.Random.Range(0, colorLookupTable.Count());
                randomColorFromDict = colorLookupTable[keys[randInd]];
            }
            //Debug.Log("adding colors: " + currentColor + " and " + randomColorFromDict);

            return (currentColor + randomColorFromDict);
        }


        public abstract void ExposedColorLogic();

    }

    class ColorEqualityComparer : IEqualityComparer<Color>
    {
        public bool Equals(Color c1, Color c2)
        {
            return (
                (c1.r == c2.r) & (c1.g == c2.g) & (c1.b == c2.b));
        }

        public int GetHashCode(Color obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}

