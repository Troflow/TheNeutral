using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class Pulse : MonoBehaviour {

        List<Vector3> trajectory;
        Lite lite;
        Vector3 originPos;
        PulsePath path;
        PulseState state;

        bool isFired = false;

        public void fire(bool pNewState, List<Vector3> pTrajectory)
        {
            isFired = pNewState;
            originPos = pTrajectory[0];
            trajectory = new List<Vector3>(pTrajectory);
            setState();
        }

        public void setPath(PulsePath pPath)
        {
            path = pPath;
        }

        void move()
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

        void setState()
        {
            if (GameManager.playerBlinkState == BlinkState.EyesOpen)
            {
                state = PulseState.MovingEyesOpen;
            }
            else if (GameManager.playerBlinkState == BlinkState.EyesClosed)
            {
                state = PulseState.StaticEyesClosed;
            }
        }

        bool compareColor(Lite pLite)
        {
            return false;
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
            path.fullyTraversed();
        }

        void handlePlayerEyesState()
        {
            if (state == PulseState.MovingEyesOpen &&
                GameManager.playerBlinkState == BlinkState.EyesClosed)
            {
                state = PulseState.StaticEyesClosed;
            }
            else if (state != PulseState.MovingEyesOpen &&
                GameManager.playerBlinkState == BlinkState.EyesOpen)
            {
                state = PulseState.MovingEyesOpen;
            }
        }

        public void OnTriggerEnter(Collider pCollider)
        {
            if (pCollider.CompareTag("PulseDestination"))
            {
                var destinationColor = pCollider.GetComponent<PulsePoint>().getLite();

                // Close path only if pulse is colored same as its destination
                if (compareColor(destinationColor))
                {
                    path.close();
                }
            }
        }

        public void OnTriggerStay(Collider pCollider)
        {
            if (pCollider.CompareTag("Player-Sphere"))
            {
                var playerState = pCollider.GetComponentInParent<PlayerState>();
				var playerActionState = playerState.getPlayerActionState();

                if (playerActionState == PlayerActionState.Attacking &&
                    state == PulseState.StaticEyesClosed)
                {
                    state = PulseState.MovingEyesClosed;
                }
            }
        }

        void Update()
        {

            handlePlayerEyesState();

            if (isFired && state != PulseState.StaticEyesClosed)
            {
                move();
            }
        }
	}
}