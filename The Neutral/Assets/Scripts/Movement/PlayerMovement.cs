using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class PlayerMovement : MonoBehaviour, IMovementBase
    {
        public UnityEngine.AI.NavMeshAgent agent { get; set; }

        public PlayerMovement(UnityEngine.AI.NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public int Move()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.GetPoint(500), Color.red);
            if (Input.GetMouseButtonDown(0))
            {
				if (Physics.Raycast(ray, out hit, 500f))
                {
                    print(hit.collider.tag);
                    //var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //sphere.transform.position = hit.point;
                    //sphere.transform.localScale += new Vector3(5, 5, 5);
                    UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();

                    bool hasFoundPath = agent.CalculatePath(hit.point, path);
                    Debug.Log(agent.updateRotation);

                    if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                    {
                        agent.SetPath(path);
                        return (int)UnityEngine.AI.NavMeshPathStatus.PathComplete;
                        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.point), Time.deltaTime);
                    }
                    else if (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial)
                    {
                        agent.SetPath(path);
                        return (int)UnityEngine.AI.NavMeshPathStatus.PathPartial;
                    }
                    else if (path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                    {
                        return (int)UnityEngine.AI.NavMeshPathStatus.PathInvalid;
                    }

                }
                else
                {
                    print("NOT HIT");
                }
            }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (Physics.Raycast(ray, out hit, 100))
            //    {
            //        NavMeshHit navmeshHit;
            //        int walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
            //        if (NavMesh.SamplePosition(hit.point, out navmeshHit, 1.0f, walkableMask))
            //        {
            //            agent.SetDestination(navmeshHit.position);
            //        }
            //    }
            //}

            return -1;
        }
    }

}