using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral {

    public static class CombatHelper
    {

        public static IEnumerator SphereExpandAndRetractFromCurrent(GameObject sphere, float tierExpansionRate, float maxSphereScale)
        {

            //assume x,y,z are same scale as sphere shouldn't distort
            float currentSphereScale = sphere.transform.localScale.x;
            
            while (sphere.transform.localScale.y < maxSphereScale)
            {
                sphere.transform.localScale += new Vector3(tierExpansionRate, tierExpansionRate, tierExpansionRate);
                yield return new WaitForFixedUpdate();
            }
            while (sphere.transform.localScale.y > currentSphereScale)
            {
                sphere.transform.localScale -= new Vector3(tierExpansionRate, tierExpansionRate, tierExpansionRate);
                yield return new WaitForFixedUpdate();
            }

        }


        public static IEnumerator SphereRetractFromCurrentToDefault(GameObject sphere, float tierExpansionRate, float defaultSphereScale)
        {
            while (sphere.transform.localScale.y > defaultSphereScale)
            {
                sphere.transform.localScale -= new Vector3(tierExpansionRate, tierExpansionRate, tierExpansionRate);
                yield return new WaitForFixedUpdate();
            }
        }

    }
}



