using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral {
	
	public class CombatController : MonoBehaviour {
        private string name;
		private List<Tier> tierList;
		private Animator anim;
        private GameObject sphere;
		private bool isDelayCoRoutineRunning;
		private bool isDelayFinished;
		private float idleReadyDurationValue;
		private bool idleStateHasChanged;
        public bool isAnimationFinished;
        private bool skipMoveCombinationCheck;

        private bool timerStart;

        //private IMovementBase movement;

        private bool combatAnimationInitiated;
        private bool animationPlayed;
        int isCombat = Animator.StringToHash("isCombat");
        private Tier currentTierPlaying;

        private bool debug = false;

        public void SetCombatControllerDefaults(List<Tier> tierList, Animator anim, GameObject sphere, string name) {
            this.name = name;
			this.tierList = tierList;
			this.anim = anim;
            //this.movement = movement;
            this.sphere = sphere;
            if (this.sphere == null) print("SPHERE NOT FOUND");

            idleReadyDurationValue = 0f;
            idleStateHasChanged = false;
            timerStart = false;
            combatAnimationInitiated = false;
            animationPlayed = false;
            isDelayCoRoutineRunning = false;
            isDelayFinished = false;
            isAnimationFinished = false;

            skipMoveCombinationCheck = false;
  
            currentTierPlaying = new Tier();
        }

        private void ResetAllCombatTriggers()
        {
            //anim.SetBool(isCombat, false);
            for (int x = 0; x < tierList.Count; x++)
            {
                anim.ResetTrigger(tierList[x].AnimationHash);
            }
            //anim.ResetTrigger(isCounterState);
            combatAnimationInitiated = false;
        }

        private IEnumerator SphereWithTime(float tierExpansionRate)
        {

            while (sphere.transform.localScale.y < 31)
            {
                //Vector3 currScale = sphere.transform.localScale;
                //currScale.Set(currScale.x + tierExpansionRate, currScale.y + tierExpansionRate, currScale.z + tierExpansionRate);
                //sphere.transform.localScale = currScale;
                //print(sphere.transform.localScale.y);
                sphere.transform.localScale += new Vector3(tierExpansionRate, tierExpansionRate, tierExpansionRate);
                yield return new WaitForFixedUpdate();
            }
            while (sphere.transform.localScale.y > 13)
            {
                sphere.transform.localScale -= new Vector3(tierExpansionRate, tierExpansionRate, tierExpansionRate);
                //Vector3 currScale = sphere.transform.localScale;
                //currScale.Set(currScale.x - tierExpansionRate, currScale.y - tierExpansionRate, currScale.z - tierExpansionRate);
                //sphere.transform.localScale = currScale;
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator WaitForDelay(float seconds)
        {
            isDelayCoRoutineRunning = true;
            yield return new WaitForSeconds(seconds);
            if (debug) Debug.Log("DONE");
            isDelayCoRoutineRunning = false;
            isDelayFinished = true;
        }

        public void checkForMoveCombination(AnimatorStateInfo stateInfo) {

            //if there's an animation playing, no need to check for anything
            if (combatAnimationInitiated) skipMoveCombinationCheck = true;

			foreach (var move in tierList) {

                if (skipMoveCombinationCheck) break;

				if (Input.GetKeyDown (move.Combo[0].ToString ()) && move.ComboPressed.Count == 0) {
					move.ComboPressed.Add (move.Combo[0].ToString());
				}

				if (move.ComboPressed.Count == 1) {
					if (move.ComboPressed[0].Equals(move.Combo[0].ToString ())) {
						if (!move._IsInCoroutine) {
                            if (debug) Debug.Log("starting coroutine");
                            StartCoroutine(move.CombatSequnce(Time.time));
						}
					}	
				}
				if (move.ComboActivated)
				{
					//if you have queued up a move, then try to use another move during the delay period
					//add in baseDelay
					if (!isDelayCoRoutineRunning && !isDelayFinished) StartCoroutine(WaitForDelay(move.ExciteDelay));
					else if (isDelayCoRoutineRunning)
					{
						continue;
					}
                    if (debug) Debug.Log(isDelayFinished);
					if (isDelayFinished)
					{
                        if (name == "Player")
                        {
                            if (PlayerMovement.inControl(true)) {
                                //reset the path so that it stops calculating
                                PlayerMovement.agent.ResetPath();
                            }
                        }
                        else
                        {
                            if (PlayerMovement.inControl(false)) {
                                //reset the path so that it stops calculating
                                PlayerMovement.agent.ResetPath();
                            }
                        }

						currentTierPlaying = move;
						combatAnimationInitiated = true;

                        //set the combat to true so the animation goes straight from run -> combat
                        //anim.SetBool(isCombat, true);
                        //set the speed to zero which implies no more movement therefore no more running
                        PlayerMovement.agent.velocity = Vector3.zero;

						anim.SetTrigger(move.AnimationHash);

						//clears all combo pressed
						tierList.ForEach(x => x.ComboPressed.Clear());
                        //print("MOVE EXPANSION RATE: " + move.ExpansionRate);
						break;
					}
					else
					{
						//this block is when we are in the delay phase
					}

				}

				if (move.ComboPressed.Count > 0)
				{
					//they are in mid-keypress of a combo
				}



			}//foreach move


            if (combatAnimationInitiated && stateInfo.IsName(currentTierPlaying.AnimationStateName))
            {
                if (debug) Debug.Log("Animation has begun playing");
                animationPlayed = true;
            }


            if (combatAnimationInitiated && animationPlayed)
            {
                StartCoroutine(SphereWithTime(currentTierPlaying.ExpansionRate));
                isAnimationFinished = true;
                //while playing animation
                for (int x = 0; x < tierList.Count; x++)
                {
                    if (tierList[x].ComboActivated)
                    {
                        //reset all variables
                        //anim.ResetTrigger(tierList[x].AnimationHash);
                        //anim.SetBool(isCombat, false);

                        combatAnimationInitiated = false;
                        animationPlayed = false;

                        tierList[x].ComboActivated = false;
                        tierList[x]._IsComboFinished = false;
                        isDelayFinished = false;
                    
                    }

                }//iterate tierList
                skipMoveCombinationCheck = false;
                ResetAllCombatTriggers();
            }//stop combat

        }

        public bool isCombatAnimationPlaying(AnimatorStateInfo stateInfo)
        {
            for (int x = 0; x < tierList.Count; x++)
            {
                if (stateInfo.IsName(tierList[x].AnimationStateName))
                {
                    return true;
                }
                else if (stateInfo.IsName("Base."+tierList[x].AnimationStateName))
                {
                    print("secondary check success");
                }

            }
            return false;
        }


        // Use this for initialization
        void Start () {

		}

		// Update is called once per frame
		void Update () {
            if (Input.GetKeyDown(KeyCode.M))
            {
                print(sphere.transform.localScale);
                sphere.transform.localScale += new Vector3(1, 1, 1);
            }
		}
	}

}

