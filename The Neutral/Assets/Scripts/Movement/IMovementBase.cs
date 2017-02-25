
using System.Collections;

namespace Neutral
{
    public interface IMovementBase
    {
        UnityEngine.AI.NavMeshAgent agent { get; set; }
        int Move();
    }

}