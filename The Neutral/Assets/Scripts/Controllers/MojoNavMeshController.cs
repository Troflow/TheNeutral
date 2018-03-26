using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    public class MojoNavMeshController : MonoBehaviour
    {

        CombatUtilities combatController;
        AnimationUtilities AnimHelper;

        private Animator anim;
        Rigidbody rb;


        int isIdleReady = Animator.StringToHash("isIdleReady");
        int isIdleReadyToIdleCalm = Animator.StringToHash("isIdleReadyToIdleCalm");
        int isRunning = Animator.StringToHash("isRunning");
        int isDashing = Animator.StringToHash("isDashing");
        int isCounterState = Animator.StringToHash("isCounterState");
        int isCounterStateSuccess = Animator.StringToHash("isCounterStateSuccess");
        int isDead = Animator.StringToHash("isDead");

        int speed = Animator.StringToHash("speed");
        int idleReadyDuration = Animator.StringToHash("idleReadyDuration");

        int isCombat = Animator.StringToHash("isCombat");


        private Tier currentTierPlaying;
        public List<Tier> tierList;


        void InitializeDefaultTierList()
        {
            tierList = new List<Tier>();
            for (int x = 0; x < 3; x++)
            {
                tierList.Add(new Tier());
            }
            tierList[0].TierLevel = 1;
            tierList[0].Combo = "zxc";
            tierList[0].AnimationHash = Animator.StringToHash("isTier1");
            tierList[0].AnimationStateName = "Tier 1";
            tierList[0].ExpansionRate = 0.6f;


            tierList[1].TierLevel = 4;
            tierList[1].Combo = "zzc";
            tierList[1].AnimationHash = Animator.StringToHash("isTier4");
            tierList[1].AnimationStateName = "Tier 4";
            tierList[1].ExpansionRate = 0.4f;


            tierList[2].TierLevel = 7;
            tierList[2].Combo = "czx";
            tierList[2].AnimationHash = Animator.StringToHash("isTier7");
            tierList[2].AnimationStateName = "Tier 7";
            tierList[2].ExpansionRate = 0.15f;


            combatController = GetComponent<CombatUtilities>();
            combatController.SetCombatUtilityDefaults(tierList, anim, GameObject.Find("Mojo-Sphere"), this.tag);
        }

        // Use this for initialization
        void Start()
        {

            anim = GetComponent<Animator>();

            AnimHelper = new AnimationUtilities();


            InitializeDefaultTierList();

        }

        void ResetAllCombatTriggers()
        {
            anim.SetBool(isCombat, false);
            for (int x = 0; x < tierList.Count; x++)
            {
                anim.ResetTrigger(tierList[x].AnimationHash);
            }
            anim.ResetTrigger(isCounterState);
        }

        // Update is called once per frame
        void Update()
        {

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            combatController.checkForMoveCombination(stateInfo);
            #region dynamic-collision-debug
            //            var sphereRadius = sphere.GetComponent<SphereCollider>().radius;
            //            var edgeOfSphere = new Vector3((sphere.transform.position.x) + (sphere.GetComponent<SphereCollider>().radius * sphere.transform.localScale.x),
            //(sphere.transform.position.y) + (sphere.GetComponent<SphereCollider>().radius * sphere.transform.localScale.y),
            //(sphere.transform.position.z) + (sphere.GetComponent<SphereCollider>().radius * sphere.transform.localScale.z));
            //            print(edgeOfSphere);
            //            var edgeX = new Vector3(edgeOfSphere.x, sphere.transform.position.y, sphere.transform.position.z);
            //            var edgeY = new Vector3(sphere.transform.position.x, edgeOfSphere.y, sphere.transform.position.z);
            //            var edgeZ = new Vector3(sphere.transform.position.x, sphere.transform.position.y, edgeOfSphere.z);
            //            Debug.DrawLine(sphere.transform.position, edgeX);
            //            Debug.DrawLine(sphere.transform.position, edgeY);
            //            Debug.DrawLine(sphere.transform.position, edgeZ);

            //            Debug.DrawLine(sphere.transform.position, GunbaiCollision.collider.transform.position);

            //            print("distance between sphereX and gunabi: " + Vector3.Distance(edgeX, GunbaiCollision.collider.transform.position));
            //            print("distance between sphereY and gunabi: " + Vector3.Distance(edgeY, GunbaiCollision.collider.transform.position));
            //            print("distance between sphereZ and gunabi: " + Vector3.Distance(edgeZ, GunbaiCollision.collider.transform.position));
            //            var dir = (GunbaiCollision.collider.transform.position - sphere.transform.position).normalized;
            //            var distanceToMultiply = (edgeZ - sphere.transform.position).magnitude;
            //            var edgeOfSphereWithGunabiDirection = sphere.transform.position + dir * distanceToMultiply;
            //            Debug.DrawLine(sphere.transform.position, edgeOfSphereWithGunabiDirection, Color.red);
            //            var edgeOfSphereToGunbaiCollider = Vector3.Distance(edgeOfSphereWithGunabiDirection, GunbaiCollision.collider.transform.position);
            //            print("dist: " + edgeOfSphereToGunbaiCollider);
            //            Debug.DrawLine(edgeOfSphereWithGunabiDirection, GunbaiCollision.collider.transform.position, Color.green);
            #endregion

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

            //rotations and animations

            if (Player.inControl(false))
            {
                if (Player.agent.hasPath)
                {
                    if (stateInfo.IsName("IdleReady"))
                    {
                        anim.SetBool(isRunning, true);
                        anim.SetBool(isIdleReady, false);
                    }

                    anim.SetFloat(speed, Mathf.Abs(Player.agent.velocity.magnitude));

                    //check if difference between destination and current position is above a certain threshold to apply rotation
                    if (Mathf.Abs((Player.agent.steeringTarget - transform.position).x) > 1.0)
                    {

                        //create a new rotation from our transform, to the difference of position of the destination and ourselves with standard time
                        var new_rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.agent.steeringTarget - transform.position), Time.deltaTime);
                        //no x or z rotation to stop tilts
                        new_rot = new Quaternion(0, new_rot.y, 0, new_rot.w);
                        transform.rotation = new_rot;
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

            if (Input.GetKeyDown(KeyCode.L))
            {
                Player.agent.ResetPath();
                anim.SetBool(isCombat, true);
                anim.SetTrigger(isCounterState);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                Player.agent.ResetPath();
                anim.SetBool(isCombat, true);
                anim.SetTrigger(isCounterStateSuccess);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                Player.agent.ResetPath();
                anim.SetBool(isCombat, true);
                anim.SetTrigger(isCounterStateSuccess);
                anim.SetBool(isDead, true);
            }

            if (stateInfo.IsName("Counter"))
            {
                ResetAllCombatTriggers();
            }
            if (stateInfo.IsName("Dazed"))
            {
                ResetAllCombatTriggers();
            }


            //if (stateInfo.IsName("IdleReady") && !idleStateHasChanged)
            //{
            //    //only set time once then compare with current running time to get len
            //    if (!timerStart)
            //    {

            //        anim.SetFloat(idleReadyDuration, Time.time);

            //        timerStart = true;
            //        anim.SetBool(isIdleReady, true);
            //    }
            //    //check if speed is fast enough to count as a run
            //    //if (Mathf.Abs(movement.agent.velocity.magnitude) > 0.1)
            //    //{
            //    //    idleStateHasChanged = true;
            //    //    timerStart = false;
            //    //    anim.SetBool(isIdleReady, false);
            //    //    anim.SetBool(isRunning, true);
            //    //}
            //    //******** BROKEN **********
            //    //check if 5 seconds have elapsed since being in ready
            //    //else if (Time.time - anim.GetFloat(idleReadyDuration) > 5f)
            //    //{
            //    //    idleStateHasChanged = true;
            //    //    timerStart = false;
            //    //    anim.SetBool(isIdleReady, false);
            //    //    anim.SetBool(isIdleReadyToIdleCalm, true);
            //    //}
            //}

            ////if (stateInfo.IsName("Run"))
            ////{
            ////    idleStateHasChanged = false;
            ////    if (Mathf.Abs(movement.agent.velocity.magnitude) < 0.1)
            ////    {
            ////        anim.SetBool(isRunning, false);
            ////        anim.SetBool(isIdleReady, true);
            ////    }
            ////}

            //if (stateInfo.IsName("IdleCalm"))
            //{
            //    idleStateHasChanged = false;
            //    if (Mathf.Abs(movement.agent.velocity.magnitude) > 0.1)
            //    {
            //        anim.SetBool(isIdleReadyToIdleCalm, false);
            //        anim.SetBool(isRunning, true);
            //    }
            //}

            //iterates through all of the moves if there is no combat animation playing


        }//Update func

    }//end class
}