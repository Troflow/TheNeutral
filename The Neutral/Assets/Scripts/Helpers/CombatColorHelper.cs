using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neutral
{
    public class CombatColorHelper
    {
        
        public static List<CombatColor> IntializePalette(EnemyType enemy)
        {
            switch (enemy)
            {
                case EnemyType.Mojo:
                    return new List<CombatColor>()
                    {
                        new CombatColorYellow(),
                        new CombatColorBlue(),
                        new CombatColorRed()
                    };

                default:
                    return new List<CombatColor>();
                  
            }
                
                
                
        }
        

    }

}
