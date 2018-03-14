using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neutral
{
	// All HUDElements must be added to the Script Execution Order to prevent
	// NullReferenceExeption Errors
	public class BlinkIcon : MonoBehaviour
	{
		private Image eyesOpenIcon;
        private Image eyesClosedIcon;

		void Start()
		{
			eyesOpenIcon = transform.Find("EyesOpen").GetComponent<Image>();
            eyesClosedIcon = transform.Find("EyesClosed").GetComponent<Image>();

            eyesOpenIcon.gameObject.SetActive(false);
            eyesClosedIcon.gameObject.SetActive(false);
		}

        void Update()
        {
            eyesOpenIcon.gameObject.SetActive(GameManager.playerBlinkState == BlinkState.EyesOpen);
            eyesClosedIcon.gameObject.SetActive(GameManager.playerBlinkState == BlinkState.EyesClosed);
        }
	}
}
