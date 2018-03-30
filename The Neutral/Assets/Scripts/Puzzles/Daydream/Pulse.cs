using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class Pulse : MonoBehaviour {

        List<Vector3> trajectory;
        [SerializeField]
        Lite lite;
        Vector3 originPos;
        PulsePath path;
        [SerializeField]
        PulseState state;
        [SerializeField]
        CombatColor combatColor;
        List<CombatColor> colorBook;

        bool isFired = false;

        public void fire(bool pNewState, List<Vector3> pTrajectory)
        {
            isFired = pNewState;
            originPos = pTrajectory[0];
            trajectory = new List<Vector3>(pTrajectory);

            setState();
        }

        public void setColorProperties()
        {
            combatColor = CombatColor.liteLookupTable[lite];
            updateColor();
            colorBook = new List<CombatColor>(){
                combatColor
            };
        }

        public void setPath(PulsePath pPath)
        {
            path = pPath;
        }

        public PulseState getState()
        {
            return state;
        }

        public CombatColor getColorFromLite()
        {
            return CombatColor.liteLookupTable[lite];
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
            // TODO: Compare Pulse's lite to the given lite and return true if they match
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
            resetColoringBook();
            isFired = false;
            transform.position = originPos;
            path.fullyTraversed();
        }

        void updateColor()
        {
            transform.GetComponent<MeshRenderer>().material.color = combatColor.color.Value;
        }

        void addColorToColorBook(CombatColor pCombatColor)
        {
            colorBook.Add(pCombatColor);
            combatColor = computeColoringBookColor();
            updateColor();
        }

        CombatColor computeColoringBookColor()
        {
            var mixedColor = colorBook[0];
            for (int x = 1; x < colorBook.Count; x++)
            {
                mixedColor += colorBook[x];
            }

            return mixedColor;
        }

        void resetColoringBook()
        {
            combatColor = CombatColor.liteLookupTable[lite];
            colorBook.Clear();
            colorBook.Add(combatColor);
            updateColor();
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

            if (pCollider.CompareTag("Pulse"))
            {
                var otherPulse = pCollider.GetComponent<Pulse>();
                if (state == PulseState.MovingEyesClosed &&
                    otherPulse.getState() == PulseState.StaticEyesClosed)
                {
                    addColorToColorBook(otherPulse.getColorFromLite());
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