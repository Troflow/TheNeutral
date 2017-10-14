using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public interface IInteractionCollider
    {

        //the action to be performed
        void Interact(IList<GameObject> interactionObjects = null);

        //tells us whether or not when interacting, an action will be performed
        //in other words, it tells us if the player is within the interaction collider
        bool IsInteractAction();

    }

}

