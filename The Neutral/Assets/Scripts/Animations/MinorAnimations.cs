using UnityEngine;
using System.Collections;

public class MinorAnimations : MonoBehaviour {

    public int speed_const;
	private Animator anim;
	private Animation animationList;
	private CharacterController cont;
	private bool isShiftDown; //for setting the timer to start only during the first frame of when shift is hit
	private float slowMoTime; //the amount of time you were in "slow motion" (charging up for the dash)
	public AIPath currentMovement;

	int isDead = Animator.StringToHash("isDead");
	int isMerge = Animator.StringToHash("isMerge");
	int isRespawn = Animator.StringToHash("isRespawn");
    int speed = Animator.StringToHash("speed");
	private Vector3 moveDirection = Vector3.zero;

	void Start () {

		anim = GetComponent<Animator>();
		animationList = GetComponent<Animation> ();
		cont = GetComponent<CharacterController> ();
		currentMovement = GetComponent<AIPath>();
		isShiftDown = false;
		slowMoTime = 0f;
    }
	

	void Update () 
	{
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		anim.SetFloat(speed, Mathf.Abs (cont.velocity.magnitude));
		if (Mathf.Abs(cont.velocity.z) > 0 || Mathf.Abs(cont.velocity.x) > 0)
        {

			anim.Play("Run",0);
        }
        else if (stateInfo.IsName("Run")) {
            Debug.Log("Setting animation to idle");
            anim.Play("Idle",0);
        }

		/*
        if (stateInfo.IsName("Run"))
        {
            cont.Move(moveDirection * Time.deltaTime * speed_const);
            //transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0));
        }
*/

		if(stateInfo.IsName("Run")) {
			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				if (!isShiftDown) {
					slowMoTime = Time.time;
					anim.SetLayerWeight(1,1f);				
				}
				isShiftDown = true;

				//animationList["Run"].speed = 0.5f;
				currentMovement.speed = 2f;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift)) {
				if (isShiftDown) {
					isShiftDown = false;
					Debug.Log ("Time slow mo'd for: " + (Time.time - slowMoTime));
					currentMovement.speed = 3f;
					anim.SetLayerWeight(1,0f);
				}

				
			}
		}



		if (Input.GetKey ("z")) {
			anim.SetBool(isDead, true);
			anim.Play ("Death");
		}
		else if (Input.GetKey("c")) { 
			anim.SetBool(isMerge, true); 
			anim.Play ("Merge");
		}
		else if (Input.GetKey("x")) { 
			anim.SetBool(isRespawn, true); 
			anim.Play("Respawn");
		}
	
	
	    if (Input.GetKey ("r")) {
			anim.SetBool (isDead, false);
			anim.SetBool (isMerge, false);
			anim.SetBool (isRespawn, false);
		}


	}
	
}
