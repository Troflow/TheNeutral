using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neutral
{
    /// <summary>
    /// Partial Class responsible for handling all operations
    /// with regards to PlayerInput
    /// </summary>
    public static partial class Player
    {
        public static void HandleInput(PlayerState pPlayerState)
        {
            if (Input.GetButtonDown("Blink"))
            {
                GameManager.playerBlinkState = BlinkState.EyesClosed;
            }

            if (Input.GetButtonUp("Blink"))
            {
                GameManager.playerBlinkState = BlinkState.EyesOpen;
            }

            // For Debugging
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                pPlayerState.setPlayerActionState(PlayerActionState.Attacking);
            }

            // For Debugging
            if (Input.GetKeyUp(KeyCode.LeftBracket))
            {
                pPlayerState.setPlayerActionState(PlayerActionState.NonActing);
            }

        }
    }

}