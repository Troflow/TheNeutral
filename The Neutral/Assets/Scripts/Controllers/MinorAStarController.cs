using UnityEngine;
using System.Collections;
namespace Neutral
{
    public class MinorAStarController : MonoBehaviour
    {

        public int speed_const;
        private Animator anim;
        private CharacterController cont;
        private bool isShiftDown; //for setting the timer to start only during the first frame of when shift is hit
        private float slowMoTime; //the amount of time you were in "slow motion" (charging up for the dash)
        public AIPath currentMovement;

        int isDead = Animator.StringToHash("isDead");
        int isMerge = Animator.StringToHash("isMerge");
        int isRespawn = Animator.StringToHash("isRespawn");
        int speed = Animator.StringToHash("speed");
        private Vector3 moveDirection = Vector3.zero;

        bool AnimationFinished(AnimatorStateInfo animState)
        {
            if (animState.normalizedTime % animState.length > 0 && animState.normalizedTime >= animState.length)
            {
                return true;
            }
            return false;
        }

        void Start()
        {

            anim = GetComponent<Animator>();
            cont = GetComponent<CharacterController>();
            currentMovement = GetComponent<AIPath>();
            isShiftDown = false;
            slowMoTime = 0f;
        }


        void Update()
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            anim.SetFloat(speed, Mathf.Abs(cont.velocity.magnitude));
            if (Mathf.Abs(cont.velocity.z) > 0 || Mathf.Abs(cont.velocity.x) > 0)
            {

                anim.Play("Run", 0);
            }
            else if (stateInfo.IsName("Run"))
            {
                Debug.Log("Setting animation to idle");
                anim.Play("Idle", 0);
            }

            /*
            if (stateInfo.IsName("Run"))
            {
                cont.Move(moveDirection * Time.deltaTime * speed_const);
                //transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0));
            }
    */

            if (stateInfo.IsName("Run"))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (!isShiftDown)
                    {
                        slowMoTime = Time.time;
                        anim.SetLayerWeight(1, 1f);
                        //anim.speed = 0.5f;
                    }
                    isShiftDown = true;

                    //animationList["Run"].speed = 0.5f;
                    currentMovement.speed = 2f;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    if (isShiftDown)
                    {
                        isShiftDown = false;
                        Debug.Log("Time slow mo'd for: " + (Time.time - slowMoTime));
                        currentMovement.speed = 3f;
                        anim.SetLayerWeight(1, 0f);
                        //anim.speed = 1f;
                    }
                }
            }

            /*
            if (Input.GetKey("m"))
            {
                anim.SetBool(isSpawn, true);
                anim.Play("Spawn");
            }
            if (stateInfo.IsName("Spawn"))
            {
                Debug.Log("Normalized Time/Length: " + stateInfo.normalizedTime + "/" + stateInfo.length);
                Debug.Log("Normalized Time % length: " + stateInfo.normalizedTime % stateInfo.length);
                if (stateInfo.normalizedTime % stateInfo.length > 0 && stateInfo.normalizedTime >= stateInfo.length)
                {
                    anim.Play("Idle");
                }
            }
            */
            if (Input.GetKey("z"))
            {
                anim.SetBool(isDead, true);
                anim.Play("Death");
            }
            if (stateInfo.IsName("Death"))
            {
                if (AnimationFinished(stateInfo))
                {
                    anim.SetBool(isRespawn, true);
                }
            }


            if (Input.GetKey("c"))
            {
                anim.SetBool(isMerge, true);
                anim.Play("Merge");
            }


            if (Input.GetKey("x"))
            {
                anim.SetBool(isRespawn, true);
                anim.Play("Respawn");
            }


            if (Input.GetKey("r"))
            {
                anim.SetBool(isDead, false);
                anim.SetBool(isMerge, false);
                anim.SetBool(isRespawn, false);
            }


        }

    }
}