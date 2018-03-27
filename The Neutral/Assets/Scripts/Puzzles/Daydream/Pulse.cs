using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class Pulse : MonoBehaviour {

        List<Vector3> trajectory;
        Vector3 originPos;

        bool isInMotion = false;
        bool isFired = false;

        public void setIsFired(bool pNewState, List<Vector3> pTrajectory)
        {
            isFired = pNewState;
            originPos = pTrajectory[0];
            trajectory = new List<Vector3>(pTrajectory);
        }

        void fire()
        {
            if (trajectory.Count == 0)
            {
                resetPosition();
                return;
            }

            var targetPos = trajectory[0];
            transform.position = Vector3.MoveTowards(
                transform.position, targetPos, GameManager.pulseSpeed * Time.deltaTime);

            checkDistanceToNextPoint(targetPos);
        }

        void checkDistanceToNextPoint(Vector3 pNextPoint)
        {
            if (Vector3.Distance(transform.position, pNextPoint) <= GameManager.pulseDistanceEpsilon)
            {
                trajectory.RemoveAt(0);
            }
        }

        void resetPosition()
        {
            isFired = false;
            transform.position = originPos;
        }

        void Update()
        {
            if (isFired) fire();
        }
	}
}