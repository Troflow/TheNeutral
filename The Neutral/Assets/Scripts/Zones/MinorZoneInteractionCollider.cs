using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neutral
{
    public class MinorZoneInteractionCollider : MonoBehaviour, IInteractionCollider
    {

        bool isInteractAction;
        bool isInteractionObjectUsed;

        GameObject interactionCollider;
        GameObject interactionObject;



        public void SetCollider(GameObject collider, Vector3 colliderPosition)
        {
            interactionCollider = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            interactionCollider.name = "Attraction_Cube_Collider";
            interactionCollider.transform.SetParent(collider.transform.parent);
            interactionCollider.transform.localScale = new Vector3(15f, 15f, 15f);
            interactionCollider.transform.position = colliderPosition;
            interactionCollider.GetComponent<MeshRenderer>().enabled = false;
            interactionCollider.GetComponent<SphereCollider>().isTrigger = true;
            interactionCollider.AddComponent<InteractionTrigger>();
        }

        // Use this for initialization
        void Start()
        {
            isInteractAction = false;
            isInteractionObjectUsed = false;

            interactionObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            interactionObject.name = "Attraction_Cube";
            interactionObject.transform.SetParent(this.transform);
            interactionObject.transform.localScale = new Vector3(6f, 6f, 6f);
            interactionObject.transform.position = GetComponentInChildren<SphereCollider>().center + this.transform.position + Vector3.up * 20;
            interactionObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            RaycastHit attractionCubeColliderPosition;

            if (Physics.Raycast(interactionObject.transform.position, Vector3.down, out attractionCubeColliderPosition))
            {
                SetCollider(interactionObject, attractionCubeColliderPosition.point);
            }
            else
            {
                print("RAYCAST ISSUE");
            }
        }

        // Update is called once per frame
        void Update()
        {
            Debug.DrawRay(interactionObject.transform.position, Vector3.down * 100);
            if (!isInteractionObjectUsed)
            {
                interactionObject.transform.Rotate(Vector3.right + Vector3.up, 50f * Time.deltaTime, Space.World);
            }
            isInteractAction = interactionCollider.GetComponent<InteractionTrigger>().isInteractingInInteractionCollider();
        }

        //the interaction objects are the minor gameobjects within the zone
        public void Interact(IList<GameObject> interactionObjects)
        {
            if (isInteractionObjectUsed)
            {
                return;
            }

            StartCoroutine(MoveMinorsTowardsAttractionCube(interactionObjects));

            print("INTERACTING");

        }

        public bool IsInteractAction()
        {
            return isInteractAction;
        }


        IEnumerator MoveMinorsTowardsAttractionCube(IList<GameObject> interactionObjects)
        {
            isInteractionObjectUsed = true;
            float startTime = Time.time;
            List<Vector3> currentDestinations = new List<Vector3>();
            foreach (GameObject minor in interactionObjects)
            {
                minor.GetComponent<MinorStatePatternEnemy>().stopAI();
                minor.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = minor.GetComponent<UnityEngine.AI.NavMeshAgent>().speed * 2;
                minor.GetComponentInChildren<SphereCollider>().enabled = false;
                currentDestinations.Add(minor.GetComponent<UnityEngine.AI.NavMeshAgent>().destination);
                minor.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(interactionCollider.transform.position);
            }

            yield return new WaitForSeconds(5f);


            for (int x=0; x<interactionObjects.Count; x++)
            {
                interactionObjects[x].GetComponent<MinorStatePatternEnemy>().resumeAI();
                interactionObjects[x].GetComponentInChildren<SphereCollider>().enabled = true;
                interactionObjects[x].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(currentDestinations[x]);
            }
            print("waiting 5 seconds for cube to resume");
            yield return new WaitForSeconds(5f);
            print("cube resume");
            isInteractionObjectUsed = false;
        }

    }

}
