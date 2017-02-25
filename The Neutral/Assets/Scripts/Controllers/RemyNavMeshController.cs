using UnityEngine;
using System.Collections;


namespace Neutral {
	public class RemyNavMeshController : MonoBehaviour {
		
		private Animator anim;
		IMovementBase movement;

		AnimationUtilities AnimHelper;
		Rigidbody rb; 
		private GameObject sphere;


		int isIdleReady = Animator.StringToHash("isIdle");
		int isRunning = Animator.StringToHash("isRunning");
		int isDashing = Animator.StringToHash("isDashing");
		int isExhausted = Animator.StringToHash("isExhausted");
		int speed = Animator.StringToHash("speed");



		// Use this for initialization
		void Start () {
			movement = new PlayerMovement(GetComponent<UnityEngine.AI.NavMeshAgent>());
			anim = GetComponent<Animator>();
			movement.agent.updateRotation = false;
			AnimHelper = new AnimationUtilities();

		}

		// Update is called once per frame
		void Update () {
			
			int pathStatus = movement.Move();
			if (pathStatus == 0)
			{
				print("The agent can reach the destionation");
			}
			else if (pathStatus == 1)
			{
				print("The agent can only get close to the destination");
			}
			else if (pathStatus == 2)
			{
				print("The agent cannot reach the destination");
				print("hasFoundPath will be false");
			}
			else
			{
				//no movement occured
			}

			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
			if (movement.agent.hasPath)
			{
				if (anim.GetBool("isRunning") == false)
				{
					anim.SetBool(isRunning, true);
					anim.SetBool(isIdleReady, false);
				}

				anim.SetFloat(speed, Mathf.Abs(movement.agent.velocity.magnitude));
				print (Mathf.Abs (movement.agent.velocity.magnitude));

				//check if difference between destination and current position is above a certain threshold to apply rotation
				if (Mathf.Abs((movement.agent.steeringTarget - transform.position).x) > 0.5)
				{

					//create a new rotation from our transform, to the difference of position of the destination and ourselves with standard time
					var new_rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.agent.steeringTarget - transform.position), Time.deltaTime);
					//no x or z rotation to stop tilts
					new_rot = new Quaternion(0, new_rot.y, 0, new_rot.w);
					transform.rotation = new_rot;
				}

			}

			if (movement.agent.remainingDistance <= movement.agent.stoppingDistance)
			{
				if (movement.agent.velocity.sqrMagnitude == 0f)
				{
					movement.agent.ResetPath();
					anim.SetBool(isRunning, false);
					anim.SetBool(isIdleReady, true);
				}
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				anim.SetTrigger(isDashing);
			}

			if (Input.GetKeyDown (KeyCode.E)) {
				movement.agent.ResetPath();
				anim.SetBool (isExhausted, true);
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				anim.SetBool (isExhausted, false);
			}
		}
	}

}
