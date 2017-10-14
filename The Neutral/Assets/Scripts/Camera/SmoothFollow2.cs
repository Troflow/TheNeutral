using UnityEngine;
using System.Collections;

namespace Neutral
{
    public class SmoothFollow2 : MonoBehaviour
    {
        public Transform target;

        private Vector3 startCamPos = new Vector3();
        private Quaternion startCamRot = Quaternion.identity;

        private void Start()
        {
            startCamPos = this.transform.localPosition;
            startCamRot = this.transform.localRotation;
        }

        void Update()
        {
                        
            if (Input.GetMouseButton(1))
            {
                transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * 5f);
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.localPosition = startCamPos;
                transform.localRotation = startCamRot;
            }

        }
    }
}
