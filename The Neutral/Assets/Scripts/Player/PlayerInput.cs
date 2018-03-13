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
        public static void HandleInput()
        {
            if (Input.GetButtonDown("Blink"))
            {
                GameManager.playerEyesOpen = false;
            }

            if (Input.GetButtonUp("Blink"))
            {
                GameManager.playerEyesOpen = true;
            }

        }
    }

}