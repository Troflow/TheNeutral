using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    public class Tier : MonoBehaviour
    {
        public static bool _IsComboFinished = false;
        public const float BaseDelay = 0.2f;

        private int _TierLevel;
        public int TierLevel
        {
            get
            {
                return _TierLevel;
            }
            set
            {
                _TierLevel = value;
                this.ExciteDelay = this.TierLevel * BaseDelay;

            }
        }
        
        public float ExpansionRate { get; set; }

        public string Combo { get; set; }
        public float ExciteDelay { get; set; }
        public bool IsDelayFinished { get; set; }

        public List<string> ComboPressed;
        public bool ComboActivated;

        public int AnimationHash;
        public string AnimationStateName;

        public bool _IsInCoroutine;

        public Tier()
        {
            ComboPressed = new List<string>();
            ComboActivated = false;
            _IsInCoroutine = false;
            IsDelayFinished = false;
            ExciteDelay = ExciteDelay;
        }

        public IEnumerator CombatSequnce(float timePressed)
        {
            //so that if a combo's 1st and 2nd char are the same, they both dont activate with 1 keypress
            if (_IsComboFinished)
            {

                yield return null;
            }

            else
            {
                _IsInCoroutine = true;

                yield return new WaitForEndOfFrame();

                while (Time.time - timePressed < 4f)
                {
                    if (Input.GetKeyDown(Combo[1].ToString()) && ComboPressed.Count.Equals(1))
                    {

                        ComboPressed.Add(Combo[1].ToString());
                    }
                    if (Input.GetKeyDown(Combo[2].ToString()) && ComboPressed.Count.Equals(2))
                    {

                        ComboPressed.Add(Combo[2].ToString());

                        ComboActivated = true;
                        _IsComboFinished = true;
                        
                        ComboPressed.Clear();
                        
                        break;
                    }
                    yield return ComboPressed;
                }

                _IsInCoroutine = false;
                ComboPressed.Clear();
                yield return ComboActivated;
            }

        }
        
    }
}