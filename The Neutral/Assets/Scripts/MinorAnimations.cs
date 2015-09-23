using UnityEngine;
using System.Collections;

public class MinorAnimations : MonoBehaviour {

	public Animator anim;
	public CharacterController cont;
	// Use this for initialization
	int isDead = Animator.StringToHash("isDead");
	int isRunning = Animator.StringToHash("isRunning");
	int isMerge = Animator.StringToHash("isMerge");
	int isRespawn = Animator.StringToHash("isRespawn");
	private Vector3 moveDirection = Vector3.zero;

	void Start () {
		anim = GetComponent<Animator>();	
		cont = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);

		if (Mathf.Abs(moveDirection.z) > 0) {
			anim.SetBool (isRunning, true);
			anim.Play ("Run");
		} else
			anim.SetBool (isRunning, false);

		if (stateInfo.IsName("Run")) {

			cont.Move(moveDirection*Time.deltaTime*8);
			transform.Rotate (new Vector3(0,Input.GetAxis("Horizontal"),0));


		}
		if (Input.GetKey ("p")) {
			anim.SetBool(isDead, true);
			anim.Play ("Death");
		}
		else if (Input.GetKey("m")) { 
			anim.SetBool(isMerge, true); 
			anim.Play ("Merge");
		}
		else if (Input.GetKey("k")) { 
			anim.SetBool(isRespawn, true); 
			anim.Play("Respawn");
		}
	
	if (Input.GetKey ("r")) {
			anim.SetBool (isRunning, false);
			anim.SetBool (isDead, false);
			anim.SetBool (isMerge, false);
			anim.SetBool (isRespawn, false);
		}

	}
}
