using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Always attached to the GameObject that "forms" the zone
/// Can assume that there will always be a parent with the specific zone controller
/// </summary>
/// 

namespace Neutral
{
    public class MinorZoneHelper : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerStay(Collider col)
        {

            //change minor sphere tag so we can directly use component of detected collision
            if (col.transform.CompareTag("Minor-Sphere"))
            {
                IZone parentZone = GetComponentInParent<IZone>();
                if (parentZone.contains(col.gameObject.GetComponentInParent<MinorNavMeshController>().gameObject))
                {
                    return;
                }
                parentZone.setEntityZone(col.gameObject.GetComponentInParent<MinorNavMeshController>().gameObject);
                parentZone.add(col.gameObject.GetComponentInParent<MinorNavMeshController>().gameObject);

            }
        }
    }
}

