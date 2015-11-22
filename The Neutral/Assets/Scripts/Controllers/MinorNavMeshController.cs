using UnityEngine;
using System.Collections;

public class MinorNavMeshController : MonoBehaviour
{

    private Animator anim;

    int isDead = Animator.StringToHash("isDead");
    int isMerge = Animator.StringToHash("isMerge");
    int isRespawn = Animator.StringToHash("isRespawn");
    int speed = Animator.StringToHash("speed");


    NavMeshAgent navMeshAgent;

    void Start()
    {

        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                navMeshAgent.SetDestination(hit.point);
            }
        }


        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        Debug.Log(Mathf.Abs(navMeshAgent.velocity.magnitude));
        anim.SetFloat(speed, Mathf.Abs(navMeshAgent.velocity.magnitude));
        Debug.Log("Anim speed : " + anim.GetFloat(speed));
        if (Mathf.Abs(navMeshAgent.velocity.magnitude) > 0.5)
        {
            anim.Play("Run", 0);
        }
        else if (stateInfo.IsName("Run"))
        {
            Debug.Log("Setting animation to idle");
            anim.Play("Idle", 0);
        }


        if (Input.GetKey("z"))
        {
            anim.SetBool(isDead, true);
            anim.Play("Death");
        }
        else if (Input.GetKey("c"))
        {
            anim.SetBool(isMerge, true);
            anim.Play("Merge");
        }
        else if (Input.GetKey("x"))
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