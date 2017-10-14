using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{
    public class InteractionTrigger : MonoBehaviour
    {

        bool isInInteractionCollider = false;
        bool isInteractAction = false;

        void OnTriggerEnter(Collider col)
        {
            print(col.tag + " hit interaction collider");
            if (col.CompareTag("Player-Sphere"))
            {
                isInInteractionCollider = true;
                print("hit interaction collider");
            }
        }

        void OnTriggerStay(Collider col)
        {
            if (col.CompareTag("Player-Sphere"))
            {
                Animator anim = col.gameObject.transform.parent.GetComponent<Animator>();
                AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Remy | Interaction"))
                {
                    isInteractAction = true;
                }
                else
                {
                    isInteractAction = false;
                }
            }
        }

        void OnTriggerExit(Collider col)
        {
            print(col.tag + " left interaction collider");
            if (col.CompareTag("Player-Sphere"))
            {
                isInInteractionCollider = false;
                print("left interaction collider");
            }

        }

        public bool isInInteractCollider()
        {
            return isInInteractionCollider;
        }

        public bool isInteractingInInteractionCollider()
        {
            return isInteractAction;
        }
    }

}
