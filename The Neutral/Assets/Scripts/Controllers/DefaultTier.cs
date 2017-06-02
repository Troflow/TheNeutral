using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    public class DefaultTier : MonoBehaviour
    {
        public static bool isComboFinished = false;


        public int tier { get; set; }
        public string combo { get; set; }
        public List<string> comboPressed;
        public bool comboActivated;

        public int animationHash;
        public string animationStateName;

        public bool isInCoroutine;

        public DefaultTier()
        {
            comboPressed = new List<string>();
            comboActivated = false;
            isInCoroutine = false;
        }

        public IEnumerator CombatSequnce(float timePressed)
        {
            //so that if a combo's 1st and 2nd char are the same, they both dont activate with 1 keypress
            if (isComboFinished)
            {
                Debug.Log("RETURNING NULL");
                yield return null;
            }

            else
            {
                isInCoroutine = true;

                yield return new WaitForEndOfFrame();

                while (Time.time - timePressed < 4f)
                {
                    if (Input.GetKeyDown(combo[1].ToString()) && comboPressed.Count.Equals(1))
                    {
                        Debug.Log("Pressed second key in sequence");
                        comboPressed.Add(combo[1].ToString());
                    }
                    if (Input.GetKeyDown(combo[2].ToString()) && comboPressed.Count.Equals(2))
                    {
                        Debug.Log("Pressed third key in sequence");
                        comboPressed.Add(combo[2].ToString());

                        comboActivated = true;
                        isComboFinished = true;
                        
                        comboPressed.Clear();
                        
                        break;
                    }
                    yield return comboPressed;
                }
                Debug.Log("Coroutine ends at end");
                isInCoroutine = false;
                comboPressed.Clear();
                yield return comboActivated;
            }

        }
        
    }
}