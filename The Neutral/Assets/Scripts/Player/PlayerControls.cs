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
        private static Blink playerBlink;

        public static int animationID;
        public static bool moveButtonPressed;

        static int isDashing;
        static int isExploit;
        static int isCounterState;
        static int isInteraction;

        public static void SetInitialInputValues(Dictionary<string, int> pAnimationIDS, Blink pBlink)
        {
            playerBlink = pBlink;
            isDashing = pAnimationIDS["isDashing"];
            isExploit = pAnimationIDS["isExploit"];
            isCounterState = pAnimationIDS["isCounterState"];
            isInteraction = pAnimationIDS["isInteraction"];
        }

        public static void HandleInput()
        {
            animationID = 0;
            moveButtonPressed = false;

            if (Input.GetButtonDown("Move"))
            {
                moveButtonPressed = true;
			}
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
            if (Input.GetButtonDown("Color_Switch"))
            {
                // Respond Accordingly
            }
            if (Input.GetButtonDown("Bow"))
            {
                animationID = isInteraction;
            }

            if (Input.GetButtonDown("Blink"))
            {
                playerBlink.closeEyes();
			}
            if (Input.GetButtonUp("Blink"))
            {
                playerBlink.openEyes();
            }
        }
    }
}