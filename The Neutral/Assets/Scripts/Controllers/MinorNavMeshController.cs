using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MinorNavMeshController : MonoBehaviour
    {

        private Animator anim;
        UnityEngine.AI.NavMeshAgent navMeshAgent;
        AnimationUtilities AnimHelper;
        int isDead = Animator.StringToHash("isDead");
        int isMerge = Animator.StringToHash("isMerge");
        int isRespawn = Animator.StringToHash("isRespawn");
        int speed = Animator.StringToHash("speed");

        void ResetAnimatorParameters()
        {
            anim.SetBool(isDead, false);
            anim.SetBool(isMerge, false);
            anim.SetBool(isRespawn, false);
        }
        void Start()
        {

            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            AnimHelper = new AnimationUtilities();
        }


        void Update()
        {
           
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            anim.SetFloat(speed, Mathf.Abs(navMeshAgent.velocity.magnitude));
            if (Mathf.Abs(navMeshAgent.velocity.magnitude) > 0.5)
            {
                anim.Play("Run", 0);
            }
            else if (stateInfo.IsName("Run"))
            {
                anim.Play("Idle", 0);
            }


            if (Input.GetKey("z"))
            {
                anim.SetBool(isDead, true);
                anim.Play("Death");
            }
            if (stateInfo.IsName("Death"))
            {
                if (AnimHelper.AnimationFinished(stateInfo))
                {
                    anim.SetBool(isRespawn, true);
                    //no need to do anim.Play("Idle") as we want animation to finish and state machine to take care of playing next one.
                }
            }
            if (stateInfo.IsName("Respawn"))
            {
                if (AnimHelper.AnimationFinished(stateInfo))
                {
                    anim.SetBool(isRespawn, false);
                    anim.SetBool(isDead, false);
                }
            }


            if (Input.GetKey("x"))
            {
                anim.SetBool(isMerge, true);
                anim.Play("Merge");
            }

    //        if (stateInfo.IsName("Merge")) 	
    //        {
				//Debug.Log("Modified merge time: " + (stateInfo.length-2.5f));
    //            if (AnimHelper.AnimationFinished(stateInfo,stateInfo.length-2.5f))
    //            {
				//	Debug.Log ("SETTING MERGE TO FALSE");
    //                anim.SetBool(isMerge, false);
    //            }
    //        }

			if (Input.GetKeyDown ("p")) {
				Vector3 centerPosition = new Vector3(this.transform.position.x - 0.5f, 0, this.transform.position.z-28f);
				//navMeshAgent.SetDestination(centerPosition);
				StartCoroutine("ForceMove");
			}

            //uncomment if you want to test respawn animation alone with a key, otherwise it is played after death animation automatically
            /*
            if (Input.GetKey("c"))
            {
                anim.SetBool(isRespawn, true);
                anim.Play("Respawn");
            }
            */


        }

    }
}