using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public interface IBlinkable
    {
        /// <summary>
        /// Handles context-specific behaviour for when the Player's
        /// eyes are either opened or closed
        /// </summary>
        void handlePlayerEyesState();
    }

}
