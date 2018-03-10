using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public interface IInteractable
    {
        // Boolean to prevent Interact() from being called multiple frames
        bool IsBeingInteractedWith { get; set; }

        /// <summary>
		/// Interface Method for all Interactable Elements in-game, allowing for context-sensitive
        /// events to occur.
		/// </summary>
        void Interact();
    }

}

