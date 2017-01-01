using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class GunbaiCollision : MonoBehaviour
    {

        public static SphereCollider collider;
        // Use this for initialization
        void Start()
        {
            collider = GetComponent<SphereCollider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision trigger detected on gunbai");
        }
        void OnCollisionEnter(Collision other)
        {
            Debug.Log("Collision real detected on gunbai");
        }
    }
}
