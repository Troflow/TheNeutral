using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class AIMovement : MonoBehaviour, IMovementBase
    {
        public NavMeshAgent agent { get; set; }
        public AIMovement(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public int Move()
        {
            NavMeshPath path = new NavMeshPath();

            //agent.CalculatePath(this.transform.position*)
            return 0;
            //bool hasFoundPath = agent.CalculatePath(,path);
        }

    }

}
