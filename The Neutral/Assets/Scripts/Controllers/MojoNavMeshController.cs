using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    public class MojoNavMeshController : MonoBehaviour
    {
        private Animator anim;
        NavMeshAgent navMeshAgent;
        AnimationUtilities AnimHelper;

        int isIdleReady = Animator.StringToHash("isIdleReady");
        int isIdleReadyToIdleCalm = Animator.StringToHash("isIdleReadyToIdleCalm");
        int isRunning = Animator.StringToHash("isRunning");
        int isDashing = Animator.StringToHash("isDashing");
        int isCounterState = Animator.StringToHash("isCounterState");
        int isCounterStateSuccess = Animator.StringToHash("isCounterStateSuccess");

        int speed = Animator.StringToHash("speed");
        int idleReadyDuration = Animator.StringToHash("idleReadyDuration");

        int isCombat = Animator.StringToHash("isCombat");
        
        private float idleReadyDurationValue;
        private bool idleStateHasChanged;
        private bool timerStart;

        private bool combatAnimationInitiated;
        private bool animationPlayed;

        private DefaultTier currentTierPlaying;
        public List<DefaultTier> tierList;

        void InitializeDefaultTierList()
        {
            tierList = new List<DefaultTier>();
            for (int x = 0; x < 3; x++)
            {
                tierList.Add(new DefaultTier());
            }
            tierList[0].tier = 1;
            tierList[0].combo = "asd";
            tierList[0].animationHash = Animator.StringToHash("isTier1");
            tierList[0].animationStateName = "Tier 1";

            tierList[1].tier = 4;
            tierList[1].combo = "aad";
            tierList[1].animationHash = Animator.StringToHash("isTier4");
            tierList[1].animationStateName = "Tier 4";


            tierList[2].tier = 7;
            tierList[2].combo = "das";
            tierList[2].animationHash = Animator.StringToHash("isTier7");
            tierList[2].animationStateName = "Tier 7";


        }

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            AnimHelper = new AnimationUtilities();
            idleReadyDurationValue = 0f;
            idleStateHasChanged = false;
            timerStart = false;
            combatAnimationInitiated = false;
            animationPlayed = false;

            InitializeDefaultTierList();
            currentTierPlaying = new DefaultTier();
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 500))
                {
                    navMeshAgent.SetDestination(hit.point);
                }
            }

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            anim.SetFloat(speed, Mathf.Abs(navMeshAgent.velocity.magnitude));

            if (stateInfo.IsName("IdleReady") && !idleStateHasChanged)
            {
                //only set time once then compare with current running time to get len
                if (!timerStart)
                {

                    anim.SetFloat(idleReadyDuration, Time.time);

                    timerStart = true;
                    anim.SetBool(isIdleReady, true);
                }
                //check if speed is fast enough to count as a run
                if (Mathf.Abs(navMeshAgent.velocity.magnitude) > 0.5)
                {
                    idleStateHasChanged = true;
                    timerStart = false;
                    anim.SetBool(isIdleReady, false);
                    anim.SetBool(isRunning, true);
                }
                //check if 5 seconds have elapsed since being in ready
                else if (Time.time - anim.GetFloat(idleReadyDuration) > 5f)
                {
                    idleStateHasChanged = true;
                    timerStart = false;
                    anim.SetBool(isIdleReady, false);
                    anim.SetBool(isIdleReadyToIdleCalm, true);
                }
            }

            if (stateInfo.IsName("Run"))
            {
                idleStateHasChanged = false;
                if (Mathf.Abs(navMeshAgent.velocity.magnitude) < 0.5)
                {
                    anim.SetBool(isRunning, false);
                    anim.SetBool(isIdleReady, true);
                }
            }

            if (stateInfo.IsName("IdleCalm"))
            {
                idleStateHasChanged = false;
                if (Mathf.Abs(navMeshAgent.velocity.magnitude) > 0.5)
                {
                    anim.SetBool(isIdleReadyToIdleCalm, false);
                    anim.SetBool(isRunning, true);
                }
            }

            //iterates through all of the moves if there is no combat animation playing
            if (!combatAnimationInitiated)
            {
                //Debug.Log("TAKING INPUT FOR MOVES");
                foreach (var move in tierList)
                {
                    if (Input.GetKeyDown(move.combo[0].ToString()) && move.comboPressed.Count == 0)
                    {
                        move.comboPressed.Add(move.combo[0].ToString());
                    }
                    if (move.comboPressed.Count == 1)
                    {
                        if (move.comboPressed[0].Equals(move.combo[0].ToString()))
                        {
                            if (move.isInCoroutine) Debug.Log("Already in coroutine");
                            else
                            {
                                Debug.Log("starting coroutine");
                                StartCoroutine(move.CombatSequnce(Time.time));
                            }
                        }
                    }
                    if (move.comboActivated)
                    {
                        currentTierPlaying = move;
                        combatAnimationInitiated = true;

                        anim.SetBool(isCombat, true);
                        anim.SetBool(move.animationHash, true);

                        //clears all combo pressed
                        tierList.ForEach(x => x.comboPressed.Clear());

                        break;
                    }

                    if (move.comboPressed.Count > 0)
                    {
                        Debug.Log(string.Format("move {0} has: ", move.tier));
                        move.comboPressed.ForEach(x => Debug.Log(x.ToString()));
                        Debug.Log("***********************************************");
                    }

                }//foreach move

            }

            if (combatAnimationInitiated && stateInfo.IsName(currentTierPlaying.animationStateName))
            {
                animationPlayed = true;
            }


            if (combatAnimationInitiated && stateInfo.IsName("IdleReady") && animationPlayed)
            {
                //while playing animation
                for (int x=0; x<tierList.Count; x++)
                {
                    if (tierList[x].comboActivated)
                    {
                        //reset all variables
                        anim.SetBool(tierList[x].animationHash, false);
                        anim.SetBool(isCombat, false);

                        combatAnimationInitiated = false;
                        animationPlayed = false;

                        tierList[x].comboActivated = false;
                    }

                }//iterate tierList
                DefaultTier.isComboFinished = false;
            }//stop combat

        }//Update func

    }//end class
}