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
            colorWheel = GameObject.Find("ColorWheel");
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
                colorWheel.SetActive(true);
                colorWheel.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 500))
                {
                    if (hit.transform.name.Contains("ColorWheel"))
                    {
                        if (hit.transform.name.Contains("RED"))
                        {
                            target.GetComponent<PlayerState>().setCurrentCombatColor(new CombatColorRed());
                        }
                        if (hit.transform.name.Contains("GREEN"))
                        {
                            target.GetComponent<PlayerState>().setCurrentCombatColor(new CombatColorGreen());
                        }
                        if (hit.transform.name.Contains("BLUE"))
                        {
                            target.GetComponent<PlayerState>().setCurrentCombatColor(new CombatColorBlue());
                        }
                    }
                }
                colorWheel.SetActive(false);
            }

        }
    }
}
