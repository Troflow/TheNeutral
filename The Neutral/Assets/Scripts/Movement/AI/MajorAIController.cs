using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class MajorAIController : MonoBehaviour
    {
        IMovementBase movement;

        // Use this for initialization
        void Start()
        {
            movement = new AIMovement(GetComponent<NavMeshAgent>());
        }

        // Update is called once per frame
        void Update()
        {
            movement.Move();
        }
    }

}