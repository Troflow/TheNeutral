using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
	public class Pulse : MonoBehaviour {

        int trajectoryLength;
        int passedPointsCount;
        List<Vector3> originalTrajectory;
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

        #region ACTIVATION

        public void activate(PulsePath pPath)
        {
            path = pPath;
            originalTrajectory = new List<Vector3>(path.getPoints());
            trajectory = new List<Vector3>(path.getPoints());
            trajectoryLength = trajectory.Count;
            originPos = trajectory[0];
            passedPointsCount = 0;
        }

        public void setColorProperties()
        {
            combatColor = CombatColor.liteLookupTable[lite];
            updateColor();
            colorBook = new List<CombatColor>(){
                combatColor
            };
        }

        #endregion

        public void deactivate()
        {
            isFired = false;
            transform.position = originPos;

            combatColor = CombatColor.liteLookupTable[Lite.WHITE];
            updateColor();

            originalTrajectory = null;
            trajectory = null;
            trajectoryLength = 0;
            passedPointsCount = 0;

            colorBook = null;
            combatColor = null;

            path = null;
        }

        public void fire()
        {
            isFired = true;
            setState();
        }

        public PulseState getState()
        {
            return state;
        }

        public CombatColor getCombatColorFromLite()
        {
            return CombatColor.liteLookupTable[lite];
        }

        void move()
        {
            if (passedPointsCount == trajectoryLength)
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
            var destinationCombatColor = CombatColor.liteLookupTable[pLite];
            return combatColor.color.Value == destinationCombatColor.color.Value;
        }

        void checkDistanceToNextPoint(Vector3 pNextPoint)
        {
            if (Vector3.Distance(transform.position, pNextPoint) <= GameManager.pulseDistanceEpsilon)
            {
                // Move the closestPoint to the back of the list, by removing then adding again
                var closestPoint = trajectory[0];
                trajectory.RemoveAt(0);
                trajectory.Add(closestPoint);
                passedPointsCount++;
            }
        }

        void resetPosition()
        {
            resetColoringBook();
            isFired = false;
            transform.position = originPos;

            // Reset the points in the trajectory list to prevent
            // undesired behaviour
            trajectory.Clear();
            foreach (Vector3 point in originalTrajectory)
            {
                trajectory.Add(point);
            }

            passedPointsCount = 0;
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
                var destination = pCollider.GetComponent<PulseDestination>();

                // Close path only if pulse is colored same as its destination
                if (compareColor(destination.getLite()))
                {
                    destination.setIsClosed(true);
                    path.close();
                }
                // Else, reset
                else if (!destination.getIsClosed())
                {
                    resetPosition();
                }
            }

            if (pCollider.CompareTag("PulseBar"))
            {
                resetPosition();
            }

            if (pCollider.CompareTag("Pulse"))
            {
                var otherPulse = pCollider.GetComponent<Pulse>();
                if (state == PulseState.MovingEyesClosed &&
                    otherPulse.getState() == PulseState.StaticEyesClosed)
                {
                    addColorToColorBook(otherPulse.getCombatColorFromLite());
                }
            }

            if (pCollider.CompareTag("ColorDepot"))
            {
                addColorToColorBook(pCollider.GetComponent<ColorDepot>().getCombatColor());
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