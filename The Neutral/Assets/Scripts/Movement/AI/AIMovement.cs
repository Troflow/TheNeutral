using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class AIMovement : MonoBehaviour, IMovementBase
    {
        public UnityEngine.AI.NavMeshAgent agent { get; set; }
        public AIMovement(UnityEngine.AI.NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public int Move()
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();

            //agent.CalculatePath(this.transform.position*)
            return 0;
            //bool hasFoundPath = agent.CalculatePath(,path);
        }

    }

}
