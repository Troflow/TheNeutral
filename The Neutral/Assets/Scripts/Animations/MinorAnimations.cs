using UnityEngine;
using System.Collections;

public class MinorAnimations : MonoBehaviour {

    public int speed_const;
	private Animator anim;
	private CharacterController cont;


	int isDead = Animator.StringToHash("isDead");
	int isMerge = Animator.StringToHash("isMerge");
	int isRespawn = Animator.StringToHash("isRespawn");
    int speed = Animator.StringToHash("speed");
	private Vector3 moveDirection = Vector3.zero;

	void Start () {

		anim = GetComponent<Animator>();	
		cont = GetComponent<CharacterController> ();
    }
	

	void Update () 
	{
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
        anim.SetFloat(speed, Input.GetAxis("Vertical"));
        if (Mathf.Abs(moveDirection.z) > 0)
        {
            anim.Play("Run");
        }
        else if (stateInfo.IsName("Run")) {
            Debug.Log("Setting animation to idle");
            anim.Play("Idle");
        }

        if (stateInfo.IsName("Run"))
        {
            cont.Move(moveDirection * Time.deltaTime * speed_const);
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal"), 0));
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
