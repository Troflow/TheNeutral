using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral {
	
	public class CombatController : MonoBehaviour {


		private List<Tier> tierList;
		private Animator anim;
		private bool isDelayCoRoutineRunning;
		private bool isDelayFinished;
		private float idleReadyDurationValue;
		private bool idleStateHasChanged;
		private bool timerStart;

		public CombatController(List<Tier> tierList, Animator anim) {
			this.tierList = tierList;
			this.anim = anim;
		}

		/*
		public void checkForMoveCombination() {
			foreach (var move in tierList) {
				if (Input.GetKeyDown (move.Combo [0].ToString ()) && move.ComboPressed.Count == 0) {
					move.ComboPressed.Add (move.Combo [0].ToString ());
				}
				if (move.ComboPressed.Count == 1) {
					if (move.ComboPressed [0].Equals (move.Combo [0].ToString ())) {
						if (!move._IsInCoroutine) {
							//Debug.Log("starting coroutine");
							StartCoroutine (move.CombatSequnce (Time.time));
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
					print(isDelayFinished);
					if (isDelayFinished)
					{
						//reset the path so that it stops calculating
						movement.agent.ResetPath();

						currentTierPlaying = move;
						combatAnimationInitiated = true;

						//set the combat to true so the animation goes straight from run -> combat
						anim.SetBool(isCombat, true);
						//set the speed to zero which implies no more movement therefore no more running
						movement.agent.velocity = Vector3.zero;

						anim.SetTrigger(move.AnimationHash);

						//clears all combo pressed
						tierList.ForEach(x => x.ComboPressed.Clear());


						StartCoroutine(SphereWithTime(move.ExpansionRate));
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
		}
		*/
		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}
	}

}

