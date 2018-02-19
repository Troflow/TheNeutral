using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public class InteractTrigger : MonoBehaviour
    {
        IInteractable interactable;

        void Start()
        {
            interactable = transform.root.GetComponent<IInteractable>();
        }

        void OnTriggerStay(Collider pCollider)
        {
            if (pCollider.CompareTag("Player-Sphere"))
            {
                var anim = pCollider.gameObject.transform.parent.GetComponent<Animator>();
                var stateInfo = anim.GetCurrentAnimatorStateInfo(0);

                if (!interactable.IsBeingInteractedWith && stateInfo.IsName("Remy | Interaction"))
                {
                    interactable.IsBeingInteractedWith = true;
                    interactable.Interact();
                }

                if (!stateInfo.IsName("Remy | Interaction"))
                {
                    interactable.IsBeingInteractedWith = false;
                }
            }
        }

    }

}
