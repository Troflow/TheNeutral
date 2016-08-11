using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class PlayerMovement : MonoBehaviour, IMovementBase
    {
        public NavMeshAgent agent { get; set; }

        public PlayerMovement(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public int Move()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 500))
                {
                    NavMeshPath path = new NavMeshPath();

                    bool hasFoundPath = agent.CalculatePath(hit.point, path);
                    Debug.Log(agent.updateRotation);

                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.SetPath(path);
                        return (int)NavMeshPathStatus.PathComplete;
                        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.point), Time.deltaTime);
                    }
                    else if (path.status == NavMeshPathStatus.PathPartial)
                    {
                        agent.SetPath(path);
                        return (int)NavMeshPathStatus.PathPartial;
                    }
                    else if (path.status == NavMeshPathStatus.PathInvalid)
                    {
                        return (int)NavMeshPathStatus.PathInvalid;
                    }

                }
            }

            return -1;
        }
    }

}