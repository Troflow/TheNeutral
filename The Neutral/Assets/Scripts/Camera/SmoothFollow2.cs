using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class SmoothFollow2 : MonoBehaviour
    {
        public Transform target;

        private Vector3 startCamPos = new Vector3();
        private Quaternion startCamRot = Quaternion.identity;
        GameObject colorWheel;

        private void Start()
        {
            startCamPos = this.transform.localPosition;
            startCamRot = this.transform.localRotation;
            // colorWheel = GameObject.Find("ColorWheel");
        }

        void Update()
         {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.localPosition = startCamPos;
                transform.localRotation = startCamRot;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                // colorWheel.SetActive(true);
                // colorWheel.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }
    }
}
