using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public interface IStatePatternEnemy
    {
        void stopAI();

        void resumeAI();

        void setWaypoints(IList<Transform> waypoints);
    }
}