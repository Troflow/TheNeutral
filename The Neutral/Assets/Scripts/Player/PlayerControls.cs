using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    /// <summary>
    /// Partial Class that handles all input controls of the Player
    /// </summary>
    public static partial class Player
    {
        public static int animationID;

        static int isDashing;
        static int isExploit;
        static int isCounterState;
        static int isInteraction;

        public static void SetAnimationID(int pIsDashing, int pIsExploit, int pIsCounterState, int pIsInteraction)
        {
            isDashing = pIsDashing;
            isExploit = pIsExploit;
            isCounterState = pIsCounterState;
            isInteraction = pIsInteraction;
        }

        public static void HandleInput()
        {
            animationID = 0;

            if (Input.GetButtonDown("Dash"))
            {
                animationID = isDashing;
			}
            if (Input.GetButtonDown("Exploit"))
            {
                animationID = isExploit;
            }
            if (Input.GetButtonDown("CounterState"))
            {
                animationID = isCounterState;
            }
            if (Input.GetButtonDown("Bow"))
            {
                animationID = isInteraction;
            }
        }
    }
}