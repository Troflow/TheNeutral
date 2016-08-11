using UnityEngine;
using System.Collections;

namespace Neutral
{
    public interface IMovementBase
    {
        NavMeshAgent agent { get; set; }
        int Move();
    }

}