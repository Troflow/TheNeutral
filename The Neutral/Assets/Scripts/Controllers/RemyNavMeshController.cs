using UnityEngine;
using System.Collections.Generic;


namespace Neutral {
	public class RemyNavMeshController : MonoBehaviour {

        private GameObject Remy;
        public Blink playerBlink;

        private float turnSpeed = 10f;

        CombatController combatController;
		AnimationUtilities AnimHelper;
        List<Tier> tierList;

        private Animator anim;
        Rigidbody rb;

		int isIdleReady = Animator.StringToHash("isIdle");
		int isRunning = Animator.StringToHash("isRunning");
		int isDashing = Animator.StringToHash("isDashing");
		int isExhausted = Animator.StringToHash("isExhausted");
        int isCounterState = Animator.StringToHash("isCounterState");
        int isCounter = Animator.StringToHash("isCounter");
        int isInteraction = Animator.StringToHash("isInteraction");
        int isDefeated = Animator.StringToHash("isDefeated");
        int isExploit = Animator.StringToHash("isExploit");
		int speed = Animator.StringToHash("speed");

        private Dictionary<string, int> allAnimationID;

        // Use this for initialization
        void Start () {
            Remy = transform.GetChild(0).gameObject;
            playerBlink = gameObject.GetComponent<Blink>();
            playerStart();

			anim = Remy.GetComponent<Animator>();
			AnimHelper = new AnimationUtilities();

            initializeCombatController();
        }

        /// <summary>
        /// Initialises all values that the static Player class will need to function
        /// with this NavMeshController
        /// </summary>
        private void playerStart()
        {
            allAnimationID = new Dictionary<string, int>();
            allAnimationID.Add("isDashing", isDashing);
            allAnimationID.Add("isExploit", isExploit);
            allAnimationID.Add("isCounterState", isCounterState);
            allAnimationID.Add("isInteraction", isInteraction);

            Player.SetInitialMovement(gameObject);
            Player.SetInitialInputValues(allAnimationID, playerBlink);

        }

        private void initializeCombatController()
        {
            tierList = new List<Tier>();
            for (int x = 0; x < 3; x++)
            {
                tierList.Add(new Tier());
            }
            tierList[0].TierLevel = 1;
            tierList[0].Combo = "asd";
            tierList[0].AnimationHash = Animator.StringToHash("isTier1");
            tierList[0].AnimationStateName = "Tier 1";
            tierList[0].ExpansionRate = 0.9f;


            tierList[1].TierLevel = 4;
            tierList[1].Combo = "aad";
            tierList[1].AnimationHash = Animator.StringToHash("isTier4");
            tierList[1].AnimationStateName = "Tier 4";
            tierList[1].ExpansionRate = 0.7f;


            tierList[2].TierLevel = 7;
            tierList[2].Combo = "das";
            tierList[2].AnimationHash = Animator.StringToHash("isTier7");
            tierList[2].AnimationStateName = "Tier 7";
            tierList[2].ExpansionRate = 0.45f;

            //combatController = new CombatController(tierList, anim, movement);
            combatController = GetComponent<CombatController>();
            combatController.SetCombatControllerDefaults(tierList, anim, GameObject.FindGameObjectWithTag("Player-Sphere"), this.tag);
        }


        private void setTriggerAndCancelMovement(int animationId)
        {
            anim.SetTrigger(animationId);
            Player.agent.ResetPath();
            anim.SetBool(isRunning, false);
            anim.SetBool(isIdleReady, true);
        }

		// Update is called once per frame
		void Update () {

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            combatController.checkForMoveCombination(stateInfo);

            int pathStatus = Player.Move();
			if (pathStatus == 0)
			{
				//print("The agent can reach the destionation");
			}
			else if (pathStatus == 1)
			{
				//print("The agent can only get close to the destination");
			}
			else if (pathStatus == 2)
			{
				//print("The agent cannot reach the destination");
				//print("hasFoundPath will be false");
			}
			else
			{
				//no movement occured
			}

            if (Player.inControl(true)) {

                if (Player.agent.hasPath)
                {
                    if (anim.GetBool("isRunning") == false)
                    {
                        anim.SetBool(isRunning, true);
                        anim.SetBool(isIdleReady, false);
                    }

                    anim.SetFloat(speed, Mathf.Abs(Player.agent.speed));


                    //check if difference between destination and current position is above a certain threshold to apply rotation
                    if (Mathf.Abs((Player.agent.steeringTarget - Remy.transform.position).x) > 0.5)
                    {

                        //create a new rotation from our transform, to the difference of position of the destination and ourselves with standard time
                        var new_rot = Quaternion.Slerp(Remy.transform.rotation, Quaternion.LookRotation(Player.agent.steeringTarget - Remy.transform.position), Time.deltaTime * turnSpeed);
                        //no x or z rotation to stop tilts
                        new_rot = new Quaternion(0, new_rot.y, 0, new_rot.w);
                        Remy.transform.rotation = new_rot;
                    }

                }

                if (Player.agent.remainingDistance <= Player.agent.stoppingDistance)
                {
                    if (Player.agent.velocity.sqrMagnitude == 0f)
                    {
                        Player.agent.ResetPath();
                        anim.SetBool(isRunning, false);
                        anim.SetBool(isIdleReady, true);
                    }
                }
            }

            Player.HandleInput();
            if (Player.animationID == isDashing)
            {
                anim.SetTrigger(isDashing);
            }
            else if (Player.animationID != 0)
            {
                setTriggerAndCancelMovement(Player.animationID);
            }

            // if (Input.GetKeyDown(KeyCode.L))
            // {
            //     setTriggerAndCancelMovement(isDefeated);
            // }
            // if (Input.GetKeyDown(KeyCode.L))
            // {
            //     setTriggerAndCancelMovement(isCounter);
            // }
			// if (Input.GetKeyDown (KeyCode.E)) {
            //     Player.agent.ResetPath();
			// 	anim.SetBool (isExhausted, true);
			// }
        }
	}

}
